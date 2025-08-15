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
                .OrderByDescending(o => o.id)
                .ToListAsync();

            if (orders.Count == 0)
                return new ErrorResponse(false, 500, ErrorMessages.NoOrdersFound);

            return new SuccessResponse<ICollection<Orders>>(true, 200, SuccessMessages.OrdersRetrieved, orders);
        }

        public async Task<IResponses> GetOrdersByStatus(int status)
        {
            var statues = HandleFunctions.SelectOneOrMoreStatus(status);

            ICollection<Orders> orders = await _context.orders
                .Where(o => statues.Contains(o.status))
                .ToListAsync();

            if (orders.Count == 0)
                return new ErrorResponse(false, 500, ErrorMessages.NoOrdersFound);

            return new SuccessResponse<ICollection<Orders>>(true, 200, SuccessMessages.OrdersRetrieved, orders);
        }

        public async Task<IResponses> CreateOrder([FromBody] CreateOrderDto[] order)
        {
            if (order == null)
                return new ErrorResponse(false, 500, ErrorMessages.MissingOrderFields);


            List<Orders> ordersArray = new List<Orders>();
            foreach (var value in order)
            {
                var mappedOrder = value.ToCreateOrder(_context.CurrentTenantId);
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
    }
}