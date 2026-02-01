using System.Collections.Generic;

namespace ServiceSitoPanel.src.dtos.purchases
{
    public class ReadPurchaseLineDto
    {
        public string line_key { get; set; }
        public int supplier_id { get; set; }
        public string supplier_name { get; set; }
        public string code { get; set; }
        public string brand { get; set; }
        public string description { get; set; }
        public string size { get; set; }
        public int total_amount { get; set; }
        public double cost_price { get; set; }
        public double total_cost { get; set; }
        public double total_paid { get; set; }
        public string status_compra { get; set; }
        public string status_pagamento { get; set; }
        public List<int> order_ids { get; set; }
    }
}
