using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ServiceSitoPanel.src.dtos.orders;
using ServiceSitoPanel.src.model;

namespace ServiceSitoPanel.src.interfaces
{
    public interface IOrdersService
    {
        Task<IResponses> GetAllOrders();
        Task<IResponses> GetOrdersByStatus(int status);
        Task<IResponses> CreateOrder([FromBody] CreateOrderDto[] order);
        Task<IResponses> UpdateOrderStatus([FromBody] int[] orders, [FromQuery] int value);
        Task<IResponses> NewClientInOrder([FromBody] NewClientInOrderDto values);
        Task<IResponses> GetAllPendingPaidOrders();
        Task<IResponses> UpdatePricePaid([FromBody] UpdatePaidPriceDto[] dto);
    }
}