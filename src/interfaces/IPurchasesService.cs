using System.Threading.Tasks;
using ServiceSitoPanel.src.dtos.purchases;
using ServiceSitoPanel.src.interfaces;

namespace ServiceSitoPanel.src.interfaces
{
    public interface IPurchasesService
    {
        Task<IResponses> GetPurchasesWithFilters(
            string productSearch,
            int? supplierId,
            string statusCompra,
            string statusPagamento,
            int pageNumber,
            int pageSize);
        Task<IResponses> UpdatePayment(UpdatePurchasePaymentDto dto);
    }
}
