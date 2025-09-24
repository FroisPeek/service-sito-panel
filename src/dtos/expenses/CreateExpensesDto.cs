using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceSitoPanel.src.dtos.expenses
{
    public class CreateExpensesDto
    {
        public DateTime expense_date { get; set; }
        public string description { get; set; }
        public Double price { get; set; }
        public DateTime performed_at { get; set; }
        public DateTime? processed_at { get; set; }
        public DateTime? payment_date { get; set; }
    }
}