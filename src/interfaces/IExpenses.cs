using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ServiceSitoPanel.src.model;

namespace ServiceSitoPanel.src.interfaces
{
    public interface IExpenses
    {
        Task<IResponses> GetAllExpenses();
        Task<IResponses> CreateExpenses([FromBody] Expenses dto);
    }
}