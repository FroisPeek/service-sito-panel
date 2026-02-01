using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceSitoPanel.Helpers;
using ServiceSitoPanel.src.dtos.purchases;
using ServiceSitoPanel.src.interfaces;

namespace ServiceSitoPanel.src.controllers
{
    [ApiController]
    [Route("api/purchases")]
    public class PurchasesController : ControllerBase
    {
        private readonly IPurchasesService _repo;

        public PurchasesController(IPurchasesService repo)
        {
            _repo = repo;
        }

        [Authorize]
        [HttpGet("filter")]
        public async Task<IActionResult> GetPurchasesWithFilters(
            [FromQuery] string productSearch = null,
            [FromQuery] int? supplierId = null,
            [FromQuery] string statusCompra = null,
            [FromQuery] string statusPagamento = null,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await _repo.GetPurchasesWithFilters(
                productSearch,
                supplierId,
                statusCompra,
                statusPagamento,
                pageNumber,
                pageSize);

            if (!result.Flag) ResponseHelper.HandleError(this, result);

            return Ok(result);
        }

        [Authorize]
        [HttpPatch("update-payment")]
        public async Task<IActionResult> UpdatePayment([FromBody] UpdatePurchasePaymentDto dto)
        {
            var result = await _repo.UpdatePayment(dto);

            if (!result.Flag) ResponseHelper.HandleError(this, result);

            return Ok(result);
        }
    }
}
