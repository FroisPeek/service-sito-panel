using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ServiceSitoPanel.src.context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceSitoPanel.src.dtos.orders;
using ServiceSitoPanel.src.interfaces;
using ServiceSitoPanel.src.mappers;
using ServiceSitoPanel.src.model;
using static ServiceSitoPanel.src.responses.ResponseFactory;
using Minio.DataModel.Result;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Razor.TagHelpers;
using ServiceSitoPanel.src.constants;
using ServiceSitoPanel.src.functions;
using System.Reflection.Metadata;

namespace ServiceSitoPanel.src.services
{
    public class OrdersServices : IOrdersService
    {
        private readonly ApplicationDbContext _context;

        public OrdersServices(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IResponses> GetAllOrders(int pageNumber, int pageSize)
        {
            var query = _context.orders
                .Include(orders => orders.ClientJoin)
                .Where(o => o.ClientJoin != null);

            var totalCount = await query.CountAsync();

            if (totalCount == 0)
                return new ErrorResponse(false, 500, ErrorMessages.NoOrdersFound);

            var pagedOrders = await query
                .OrderByDescending(o => o.id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var mappedOrders = pagedOrders.Select(o => o.ToReadAllOrders());

            return new SuccessResponseWithPagination<ReadOrdersDto>(
                true,
                200,
                SuccessMessages.OrdersRetrieved,
                totalCount,
                pageNumber,
                pageSize,
                (int)Math.Ceiling((double)totalCount / pageSize),
                mappedOrders
            );
        }

        public async Task<IResponses> GetOrdersByStatus(int status, int pageNumber, int pageSize)
        {
            var statues = HandleFunctions.SelectOneOrMoreStatus(status);

            var query = _context.orders
                .Include(orders => orders.ClientJoin)
                .Where(o => statues.Contains(o.status));
            
            var totalCount = await query.CountAsync();

            if (totalCount == 0)
                return new ErrorResponse(false, 500, ErrorMessages.NoOrdersFound);

            var pagedOrders = await query
                .OrderByDescending(o => o.id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var mappedOrders = pagedOrders.Select(o => o.ToReadAllOrders());

            return new SuccessResponseWithPagination<ReadOrdersDto>(
                true,
                200,
                SuccessMessages.OrdersRetrieved,
                totalCount,
                pageNumber,
                pageSize,
                (int)Math.Ceiling((double)totalCount / pageSize),
                mappedOrders
            );
        }

        public async Task<IResponses> CreateOrder([FromBody] CreateOrderDto[] orders)
        {
            if (orders == null || orders.Length == 0)
                return new ErrorResponse(false, 500, ErrorMessages.MissingOrderFields);

            var clients = await _context.client.ToListAsync();

            List<Orders> ordersArray = new List<Orders>();

            foreach (var orderDto in orders)
            {
                var existingClient = clients
                    .FirstOrDefault(c => c.name.Trim().ToUpper() == orderDto.client.Trim().ToUpper());

                int clientId;

                if (existingClient != null)
                {
                    clientId = existingClient.id;
                }
                else
                {
                    var mappedClient = orderDto.ToCreateClient(_context.CurrentTenantId);
                    await _context.client.AddAsync(mappedClient);
                    await _context.SaveChangesAsync();
                    clientId = mappedClient.id;

                    clients.Add(mappedClient);
                }

                var mappedOrder = orderDto.ToCreateOrder(_context.CurrentTenantId, clientId);
                await _context.orders.AddAsync(mappedOrder);
                ordersArray.Add(mappedOrder);
            }

            await _context.SaveChangesAsync();

            return new SuccessResponse<List<Orders>>(true, 201, SuccessMessages.OrderCreated, ordersArray);
        }



        public async Task<IResponses> UpdateOrderStatus([FromBody] int[] orders, [FromQuery] int value)
        {
            if (!orders.Any())
                return new ErrorResponse(false, 500, ErrorMessages.MissingOrderCodes);

            var ordersToUpdate = await _context.orders
                .Where(o => orders.Contains(o.id))
                .ToListAsync();

            if (ordersToUpdate.Count != orders.Length)
                return new ErrorResponse(false, 404, ErrorMessages.SomeOrdersNotFound);

            foreach (var order in ordersToUpdate)
                order.UpdateOrderStatusByValue(value);

            await _context.SaveChangesAsync();
            return new SuccessResponse(true, 200, SuccessMessages.OrdersUpdated);
        }

        public async Task<IResponses> NewClientInOrder([FromBody] NewClientInOrderDto dto)
        {
            if (dto == null)
                return new ErrorResponse(false, 500, ErrorMessages.MissingOrderCodes);

            var orderToUpdate = await _context.orders
                .FirstOrDefaultAsync(o => o.id == dto.order_id);

            if (orderToUpdate == null)
                return new ErrorResponse(false, 404, ErrorMessages.NoOrdersFound);

            var mappedClient = new Client { name = dto.client, tenant_id = _context.CurrentTenantId };

            await _context.client.AddAsync(mappedClient);
            await _context.SaveChangesAsync();

            dto.MapToOrder(orderToUpdate, _context.CurrentTenantId, mappedClient.id);
            _context.orders.Update(orderToUpdate);

            await _context.SaveChangesAsync();

            return new SuccessResponse<Orders>(true, 200, SuccessMessages.OrderCreated, orderToUpdate);
        }

        public async Task<IResponses> GetAllPendingPaidOrders(int pageNumber, int pageSize)
        {
            var query = _context.orders
                .Include(o => o.ClientJoin)
                .Where(o => o.price_paid != o.total_price);

            var totalCount = await query.CountAsync();

            if (totalCount == 0)
                return new ErrorResponse(false, 404, ErrorMessages.NoOrdersFound);

            var pagedOrders = await query
                .OrderByDescending(o => o.date_creation_order)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new SuccessResponseWithPagination<Orders>(
                true,
                200,
                "Pedidos retornadas com sucesso",
                totalCount,
                pageNumber,
                pageSize,
                (int)Math.Ceiling((double)totalCount / pageSize),
                pagedOrders
            );

        }

        public async Task<IResponses> UpdatePricePaid([FromBody] UpdatePaidPriceDto[] dto)
        {
            var orderIds = dto.Select(d => d.order_id).ToList();

            var ordersToUpdate = await _context.orders
                .Where(o => orderIds.Contains(o.id))
                .ToListAsync();

            if (ordersToUpdate == null || ordersToUpdate.Count == 0)
                return new ErrorResponse(false, 404, ErrorMessages.NoOrdersFound);

            foreach (var order in ordersToUpdate)
            {
                var dtoItem = dto.First(d => d.order_id == order.id);
                var currentPaid = order.price_paid ?? 0;
                var newPaid = currentPaid + dtoItem.paid_price;
                order.price_paid = newPaid;

                // Atualiza o status automaticamente baseado no valor pago
                // Só atualiza se o pedido estiver em um status relacionado a contas a pagar
                var currentStatus = order.status;
                var isAccountsPayableStatus = currentStatus == StatusOrder.NewStatus[Status.ConfirmSale] ||
                                             currentStatus == StatusOrder.NewStatus[Status.PaidPurchase] ||
                                             currentStatus == StatusOrder.NewStatus[Status.PartialPayment] ||
                                             currentStatus == StatusOrder.NewStatus[Status.FullyPaid];

                if (isAccountsPayableStatus)
                {
                    var totalPrice = order.total_price ?? 0;

                    if (newPaid <= 0)
                    {
                        // Se não foi pago nada, mantém como Compra Realizada
                        order.status = StatusOrder.NewStatus[Status.ConfirmSale];
                    }
                    else if (newPaid >= totalPrice)
                    {
                        // Se foi totalmente pago, muda para Pagamento Quitado
                        order.status = StatusOrder.NewStatus[Status.FullyPaid];
                    }
                    else
                    {
                        // Se foi pago parcialmente, muda para Pagamento Parcial
                        order.status = StatusOrder.NewStatus[Status.PartialPayment];
                    }
                }
            }

            await _context.SaveChangesAsync();

            return new SuccessResponse<List<Orders>>(true, 200, SuccessMessages.OrderCreated, ordersToUpdate);
        }
    }
}