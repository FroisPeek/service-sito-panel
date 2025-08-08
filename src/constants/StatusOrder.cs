using ServiceSitoPanel.src.model;

namespace ServiceSitoPanel.src.constants
{

    public enum Status
    {
        PendingPurchase,
        SaleToRecive,
        ReadyForDelivery,
        ConfirmSale,
        PaidPurchase
    }

    public static class StatusOrder
    {
        public static readonly Dictionary<Status, string> NewStatus = new()
        {
            {Status.PendingPurchase, "Compra Pendente" },
            {Status.SaleToRecive, "Venda a Receber" },
            {Status.ReadyForDelivery, "Pronta a Entrega" },
            {Status.ConfirmSale, "Efetivar Venda" },
            {Status.PaidPurchase, "Compra Quitada"},
        };
    }
}