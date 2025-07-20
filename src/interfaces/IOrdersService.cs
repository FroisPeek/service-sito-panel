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
        Task<IResponses> CreateOrder([FromBody] CreateOrderDto order);
    }
}