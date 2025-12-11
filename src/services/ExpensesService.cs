using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceSitoPanel.src.constants;
using ServiceSitoPanel.src.context;
using ServiceSitoPanel.src.dtos.expenses;
using ServiceSitoPanel.src.interfaces;
using ServiceSitoPanel.src.mappers;
using ServiceSitoPanel.src.model;
using static ServiceSitoPanel.src.responses.ResponseFactory;

namespace ServiceSitoPanel.src.services
{
    public class ExpensesService : IExpenses
    {
        private readonly ApplicationDbContext _context;

        public ExpensesService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IResponses> GetAllExpenses(int pageNumber, int pageSize)
        {
            var query = _context.expenses
                .OrderByDescending(e => e.expense_date);

            var totalCount = await query.CountAsync();

            if (totalCount == 0)
                return new ErrorResponse(false, 404, "Nenhuma despesa encontrada");

            var pagedExpenses = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new SuccessResponseWithPagination<Expenses>(
                true,
                200,
                SuccessMessages.ProfilesRetrieved,
                totalCount,
                pageNumber,
                pageSize,
                (int)Math.Ceiling((double)totalCount / pageSize),
                pagedExpenses
            );
        }

        public async Task<IResponses> CreateExpenses([FromBody] CreateExpensesDto dto)
        {
            if (dto == null)
                return new ErrorResponse(false, 404, ErrorMessages.MissingOrderFields);

            var mappedExpenses = dto.ToCreateExpense(_context.CurrentTenantId);

            await _context.expenses.AddAsync(mappedExpenses);
            await _context.SaveChangesAsync();

            return new SuccessResponse<Expenses>(true, 201, "Despesas criadas com sucesso", mappedExpenses);
        }

        public async Task<IResponses> UpdateExpenses([FromBody] UpdateExpenseDto dto)
        {
            if (dto == null)
                return new ErrorResponse(false, 404, ErrorMessages.MissingOrderFields);

            var expenseToUpdate = await _context.expenses.FirstOrDefaultAsync(e => e.id == dto.id);

            if (expenseToUpdate == null)
                return new ErrorResponse(false, 404, "Despesa não encotrada");

            expenseToUpdate.description = dto.description;
            expenseToUpdate.expense_date = dto.expense_date;
            expenseToUpdate.payment_date = dto.payment_date;
            expenseToUpdate.performed_at = dto.performed_at;
            expenseToUpdate.price = dto.price;
            expenseToUpdate.processed_at = dto.processed_at;

            await _context.SaveChangesAsync();

            return new SuccessResponse<Expenses>(true, 201, "Despesa atualizada com sucesso", expenseToUpdate);
        }
    }
}