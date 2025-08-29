using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceSitoPanel.src.dtos.orders
{
    public class NewClientInOrderDto
    {
        public int order_id { get; set; }
        public string client { get; set; }
        public Double sale_price { get; set; }
        public Double total_price { get; set; }
    }
}