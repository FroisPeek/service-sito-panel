using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceSitoPanel.src.model
{
    public class Expenses
    {
        [Key]
        public int id { get; set; }
        public DateTime expense_date { get; set; }
        public string description { get; set; }
        public Double price { get; set; }
        public DateTime performed_at { get; set; }
        public DateTime? processed_at { get; set; }
        public DateTime? payment_date { get; set; }
        public int tenant_id { get; set; }
    }
}