using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceSitoPanel.src.dtos.expenses;
using ServiceSitoPanel.src.interfaces;
using ServiceSitoPanel.src.model;

namespace ServiceSitoPanel.src.controllers
{
    [ApiController]
    [Route("api/expenses")]
    public class ExpensesController : ControllerBase
    {
        private readonly IExpenses _expenses;
        public ExpensesController(IExpenses expenses)
        {
            _expenses = expenses;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetExpenses(int pageNumber = 1, int pageSize = 10)
        {
            var result = await _expenses.GetAllExpenses(pageNumber, pageSize);

            if (!result.Flag) ResponseHelper.HandleError(this, result);

            return Ok(result);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateExpenses([FromBody] CreateExpensesDto dto)
        {
            var result = await _expenses.CreateExpenses(dto);

            if (!result.Flag) ResponseHelper.HandleError(this, result);

            return Ok(result);
        }

        [Authorize]
        [HttpPatch]
        public async Task<IActionResult> UpdateExpenses([FromBody] UpdateExpenseDto dto)
        {
            var result = await _expenses.UpdateExpenses(dto);

            if (!result.Flag) ResponseHelper.HandleError(this, result);

            return Ok(result);
        }
    }
}