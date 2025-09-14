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

        public async Task<IResponses> GetAllOrders()
        {
            ICollection<Orders> orders = await _context.orders
                .Include(orders => orders.ClientJoin)
                .OrderByDescending(o => o.id)
                .ToListAsync();

            if (orders.Count == 0)
                return new ErrorResponse(false, 500, ErrorMessages.NoOrdersFound);

            var mappedOrders = orders
                .Where(o => o.ClientJoin != null)
                .Select(o => o.ToReadAllOrders());

            return new SuccessResponse<IEnumerable<ReadOrdersDto>>(true, 200, SuccessMessages.OrdersRetrieved, mappedOrders);
        }

        public async Task<IResponses> GetOrdersByStatus(int status)
        {
            var statues = HandleFunctions.SelectOneOrMoreStatus(status);

            ICollection<Orders> orders = await _context.orders
                .Include(orders => orders.ClientJoin)
                .Where(o => statues.Contains(o.status))
                .OrderBy(o => o.status)
                .ToListAsync();

            if (orders.Count == 0)
                return new ErrorResponse(false, 500, ErrorMessages.NoOrdersFound);

            var mappedOrders = orders.Select(o => o.ToReadAllOrders());

            return new SuccessResponse<IEnumerable<ReadOrdersDto>>(true, 200, SuccessMessages.OrdersRetrieved, mappedOrders);
        }

        public async Task<IResponses> CreateOrder([FromBody] CreateOrderDto[] order)
        {
            if (order == null)
                return new ErrorResponse(false, 500, ErrorMessages.MissingOrderFields);


            List<Orders> ordersArray = new List<Orders>();
            foreach (var value in order)
            {
                var mappedClient = value.ToCreateClient(_context.CurrentTenantId);
                await _context.client.AddAsync(mappedClient);
                await _context.SaveChangesAsync();

                var mappedOrder = value.ToCreateOrder(_context.CurrentTenantId, mappedClient.id);
                ordersArray.Add(mappedOrder);
                await _context.orders.AddAsync(mappedOrder);
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

        public async Task<IResponses> GetAllPendingPaidOrders()
        {
            var orders = await _context.orders
                .Include(o => o.ClientJoin)
                .Where(o => o.price_paid != o.total_price)
                .ToListAsync();

            if (orders.Count == 0)
                return new ErrorResponse(false, 404, ErrorMessages.NoOrdersFound);

            return new SuccessResponse<IEnumerable<ReadOrdersDto>>(true, 200, SuccessMessages.OrderCreated, orders.Select(o => o.ToReadAllOrders()));
        }

        public async Task<IResponses> UpdatePricePaid([FromBody] UpdatePaidPriceDto[] dto)
        {
            var orderIds = dto.Select(d => d.order_id).ToList();

            var ordersToUpdate = await _context.orders
                .Where(o => orderIds.Contains(o.id))
                .ToListAsync();

            if (ordersToUpdate == null)
                return new ErrorResponse(false, 404, ErrorMessages.NoOrdersFound);

            foreach (var order in ordersToUpdate)
            {
                var dtoItem = dto.First(d => d.order_id == order.id);
                order.price_paid = dtoItem.paid_price;
            }

            await _context.SaveChangesAsync();

            return new SuccessResponse<List<Orders>>(true, 200, SuccessMessages.OrderCreated, ordersToUpdate);
        }
    }
}