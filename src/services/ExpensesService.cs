using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ServiceSitoPanel.src.constants;
using ServiceSitoPanel.src.context;
using ServiceSitoPanel.src.interfaces;
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

        public async Task<IResponses> GetAllExpenses()
        {
            var allExpenses = await _context.expenses.ToListAsync();

            if (allExpenses.Count == 0)
                return new ErrorResponse(false, 404, ErrorMessages.NoProfilesFound);

            return new SuccessResponse<IEnumerable<Expenses>>(true, 200, SuccessMessages.ProfilesRetrieved, allExpenses);
        }
    }
}