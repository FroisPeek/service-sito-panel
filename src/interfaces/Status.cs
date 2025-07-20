using System.Runtime.Serialization;

namespace ServiceSitoPanel.src.enums
{
    public enum Status
    {
        [EnumMember(Value = "Compra Pendente")]
        CompraPendente,

        [EnumMember(Value = "Valor a Receber")]
        ValorAReceber,

        [EnumMember(Value = "Pronta Entrega")]
        ProntaEntrega,

        [EnumMember(Value = "Efetivar Venda")]
        EfetivarVenda,

        [EnumMember(Value = "Compra Quitada")]
        CompraQuitada
    }
}
