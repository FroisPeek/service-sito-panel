using ServiceSitoPanel.src.model;

namespace ServiceSitoPanel.src.constants
{

    public enum Status
    {
        PendingPurchase,
        SaleToRecive,
        ReadyForDelivery,
        ConfirmSale,
        PaidPurchase,
        ToCheck,
        Checked,
        PartialPayment,
        FullyPaid
    }

    public static class StatusOrder
    {
        public static readonly Dictionary<Status, string> NewStatus = new()
        {
            {Status.PendingPurchase, "Compra Pendente" },
            {Status.SaleToRecive, "Venda a Receber" },
            {Status.ReadyForDelivery, "Pronta a Entrega" },
            {Status.ConfirmSale, "Compra Realizada" },
            {Status.PaidPurchase, "Compra Quitada"},
            {Status.ToCheck, "A Conferir"},
            {Status.Checked, "Conferido"},
            {Status.PartialPayment, "Pagamento Parcial"},
            {Status.FullyPaid, "Pagamento Quitado"}
        };
    }
}