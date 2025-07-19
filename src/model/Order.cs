using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ServiceSitoPanel.src.interfaces;

namespace ServiceSitoPanel.src.model
{
    public class Orders
    {
        public int id { get; set; }
        public string client { get; set; }
        public int code { get; set; }
        public string? description { get; set; }
        public string size { get; set; }
        public int amount { get; set; }
        public Double cost_price { get; set; }
        public Double sale_price { get; set; }
        public Double total_price { get; set; }
        public string status { get; set; }
        public DateTime date_order { get; set; }
        public int tenant_id { get; set; }
    }
}