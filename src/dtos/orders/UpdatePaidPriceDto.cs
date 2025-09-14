using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceSitoPanel.src.dtos.orders
{
    public class UpdatePaidPriceDto
    {
        public int order_id { get; set; }
        public Double paid_price { get; set; }
    }
}