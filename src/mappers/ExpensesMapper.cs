using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ServiceSitoPanel.src.dtos.expenses;
using ServiceSitoPanel.src.model;

namespace ServiceSitoPanel.src.mappers
{
    public static class ExpensesMapper
    {
        public static Expenses ToCreateExpense(this CreateExpensesDto dto, int tenant_id)
        {
            return new Expenses
            {
                description = dto.description,
                expense_date = dto.expense_date,
                payment_date = dto.payment_date,
                performed_at = dto.performed_at,
                price = dto.price,
                processed_at = dto.processed_at,
                tenant_id = tenant_id
            };
        }
    }
}