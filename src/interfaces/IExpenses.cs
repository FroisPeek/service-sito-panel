using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ServiceSitoPanel.src.dtos.expenses;
using ServiceSitoPanel.src.model;

namespace ServiceSitoPanel.src.interfaces
{
    public interface IExpenses
    {
        Task<IResponses> GetAllExpenses(int pageNumber, int pageSize);
        Task<IResponses> CreateExpenses([FromBody] CreateExpensesDto dto);
        Task<IResponses> UpdateExpenses([FromBody] UpdateExpenseDto dto);
    }
}