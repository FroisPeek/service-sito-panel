using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using leapcert_back.src.context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceSitoPanel.src.dtos.orders;
using ServiceSitoPanel.src.interfaces;
using ServiceSitoPanel.src.mappers;
using ServiceSitoPanel.src.model;
using static ServiceSitoPanel.src.responses.ResponseFactory;

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
            ICollection<Orders> orders = await _context.orders.ToListAsync();

            if (orders.Count == 0)
                return new ErrorResponse(false, 500, "Nenhum pedido encontrado.");

            return new SuccessResponse<ICollection<Orders>>(true, 200, "Pedidos retornados com sucesso", orders);
        }

        public async Task<IResponses> CreateOrder([FromBody] CreateOrderDto[] order)
        {
            if (order == null)
                return new ErrorResponse(false, 500, "Preciso informar os campos do pedido");


            List<Orders> ordersArray = new List<Orders>();
            foreach (var value in order)
            {
                var mappedOrder = value.ToCreateOrder(_context.CurrentTenantId);
                ordersArray.Add(mappedOrder);
                await _context.orders.AddAsync(mappedOrder);
            }

            await _context.SaveChangesAsync();

            return new SuccessResponse<List<Orders>>(true, 201, "Pedido cadastrado com sucesso", ordersArray);
        }
    }
}