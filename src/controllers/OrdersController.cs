using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServiceSitoPanel.src.dtos.orders;
using ServiceSitoPanel.src.interfaces;

namespace ServiceSitoPanel.src.controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrdersService _repo;

        public OrdersController(IOrdersService repo)
        {
            _repo = repo;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllOrder()
        {
            var result = await _repo.GetAllOrders();

            if (!result.Flag) ResponseHelper.HandleError(this, result);

            return Ok(result);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto[] dto)
        {
            var result = await _repo.CreateOrder(dto);

            if (!result.Flag) ResponseHelper.HandleError(this, result);

            return Ok(result);
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdateStatusOrder([FromBody] int[] orders, [FromQuery] int value)
        {
            var result = await _repo.UpdateOrderStatus(orders, value);

            if (!result.Flag) ResponseHelper.HandleError(this, result);

            return Ok(result);
        }
    }
}