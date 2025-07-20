using ServiceSitoPanel.src.enums;

public static class StatusHelper
{
    public static string CompraPendente => Status.CompraPendente.GetEnumMemberValue();
    public static string ValorAReceber => Status.ValorAReceber.GetEnumMemberValue();
    public static string ProntaEntrega => Status.ProntaEntrega.GetEnumMemberValue();
    public static string EfetivarVenda => Status.EfetivarVenda.GetEnumMemberValue();
    public static string CompraQuitada => Status.CompraQuitada.GetEnumMemberValue();
}
