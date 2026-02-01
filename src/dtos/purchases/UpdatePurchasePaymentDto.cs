namespace ServiceSitoPanel.src.dtos.purchases
{
    public class UpdatePurchasePaymentDto
    {
        public int supplier_id { get; set; }
        public string code { get; set; }
        public string brand { get; set; }
        public string size { get; set; }
        public double paid_amount { get; set; }
    }
}
