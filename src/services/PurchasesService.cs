using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ServiceSitoPanel.src.constants;
using ServiceSitoPanel.src.context;
using ServiceSitoPanel.src.dtos.purchases;
using ServiceSitoPanel.src.interfaces;
using ServiceSitoPanel.src.model;
using static ServiceSitoPanel.src.responses.ResponseFactory;

namespace ServiceSitoPanel.src.services
{
    public class PurchasesService : IPurchasesService
    {
        private const string StatusCompraPendente = "Compra Pendente";
        private const string StatusCompraRealizada = "Compra Realizada";
        private const string StatusPagamentoPendente = "Pendente";
        private const string StatusPagamentoParcial = "Parcialmente Pago";
        private const string StatusPagamentoTotal = "Totalmente Pago";

        private readonly ApplicationDbContext _context;

        public PurchasesService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IResponses> GetPurchasesWithFilters(
            string productSearch,
            int? supplierId,
            string statusCompra,
            string statusPagamento,
            int pageNumber,
            int pageSize)
        {
            var query = _context.orders
                .Include(o => o.SupplierJoin)
                .Where(o => o.supplier != null && o.SupplierJoin != null);

            if (!string.IsNullOrWhiteSpace(productSearch))
            {
                var term = productSearch.Trim().ToLower();
                query = query.Where(o =>
                    (o.brand != null && o.brand.ToLower().Contains(term)) ||
                    (o.code != null && o.code.ToLower().Contains(term)) ||
                    (o.description != null && o.description.ToLower().Contains(term)));
            }

            if (supplierId.HasValue)
                query = query.Where(o => o.supplier == supplierId.Value);

            var orders = await query.OrderBy(o => o.supplier).ThenBy(o => o.code).ToListAsync();

            var grouped = orders
                .GroupBy(o => new { o.supplier, o.code, o.brand, o.size, SupplierName = o.SupplierJoin.name })
                .Select(g =>
                {
                    var totalAmount = g.Sum(x => x.amount);
                    var totalCost = g.Sum(x => x.amount * x.cost_price);
                    var totalPaid = g.Sum(x => x.price_paid ?? 0);
                    var first = g.First();
                    var hasPendente = g.Any(x => x.status == StatusOrder.NewStatus[Status.PendingPurchase]);
                    var statusCompraVal = hasPendente ? StatusCompraPendente : StatusCompraRealizada;
                    string statusPagamentoVal;
                    if (totalPaid <= 0) statusPagamentoVal = StatusPagamentoPendente;
                    else if (totalPaid >= totalCost) statusPagamentoVal = StatusPagamentoTotal;
                    else statusPagamentoVal = StatusPagamentoParcial;

                    return new
                    {
                        SupplierId = first.supplier.Value,
                        SupplierName = first.SupplierJoin.name,
                        Code = first.code,
                        Brand = first.brand,
                        Description = first.description ?? "",
                        Size = first.size,
                        TotalAmount = totalAmount,
                        CostPrice = first.cost_price,
                        TotalCost = totalCost,
                        TotalPaid = totalPaid,
                        StatusCompra = statusCompraVal,
                        StatusPagamento = statusPagamentoVal,
                        OrderIds = g.Select(x => x.id).ToList()
                    };
                })
                .ToList();

            var filtered = grouped.AsEnumerable();
            if (!string.IsNullOrWhiteSpace(statusCompra))
            {
                if (statusCompra == StatusCompraPendente)
                    filtered = filtered.Where(x => x.StatusCompra == StatusCompraPendente);
                else if (statusCompra == StatusCompraRealizada)
                    filtered = filtered.Where(x => x.StatusCompra == StatusCompraRealizada);
            }
            if (!string.IsNullOrWhiteSpace(statusPagamento))
            {
                if (statusPagamento == StatusPagamentoPendente)
                    filtered = filtered.Where(x => x.StatusPagamento == StatusPagamentoPendente);
                else if (statusPagamento == StatusPagamentoParcial)
                    filtered = filtered.Where(x => x.StatusPagamento == StatusPagamentoParcial);
                else if (statusPagamento == StatusPagamentoTotal)
                    filtered = filtered.Where(x => x.StatusPagamento == StatusPagamentoTotal);
            }

            var list = filtered.ToList();
            var totalCount = list.Count;

            var paged = list
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new ReadPurchaseLineDto
                {
                    line_key = $"{x.SupplierId}_{x.Code}_{x.Brand}_{x.Size}",
                    supplier_id = x.SupplierId,
                    supplier_name = x.SupplierName,
                    code = x.Code,
                    brand = x.Brand,
                    description = x.Description,
                    size = x.Size,
                    total_amount = x.TotalAmount,
                    cost_price = x.CostPrice,
                    total_cost = x.TotalCost,
                    total_paid = x.TotalPaid,
                    status_compra = x.StatusCompra,
                    status_pagamento = x.StatusPagamento,
                    order_ids = x.OrderIds
                })
                .ToList();

            return new SuccessResponseWithPagination<ReadPurchaseLineDto>(
                true,
                200,
                SuccessMessages.PurchasesRetrieved,
                totalCount,
                pageNumber,
                pageSize,
                totalCount == 0 ? 0 : (int)Math.Ceiling((double)totalCount / pageSize),
                paged);
        }

        public async Task<IResponses> UpdatePayment(UpdatePurchasePaymentDto dto)
        {
            if (dto == null || dto.paid_amount <= 0)
                return new ErrorResponse(false, 400, ErrorMessages.MissingOrderFields);

            var ordersToUpdate = await _context.orders
                .Where(o => o.supplier == dto.supplier_id &&
                    o.code == dto.code &&
                    o.brand == dto.brand &&
                    o.size == dto.size)
                .OrderBy(o => o.id)
                .ToListAsync();

            if (ordersToUpdate == null || ordersToUpdate.Count == 0)
                return new ErrorResponse(false, 404, ErrorMessages.NoPurchasesFound);

            var remaining = dto.paid_amount;
            foreach (var order in ordersToUpdate)
            {
                if (remaining <= 0) break;
                var totalPrice = order.total_price ?? 0;
                var currentPaid = order.price_paid ?? 0;
                var pending = totalPrice - currentPaid;
                if (pending <= 0) continue;
                var toAdd = Math.Min(remaining, (double)pending);
                order.price_paid = currentPaid + toAdd;
                remaining -= toAdd;

                var newPaid = currentPaid + toAdd;
                var isAccountsPayableStatus = order.status == StatusOrder.NewStatus[Status.ConfirmSale] ||
                    order.status == StatusOrder.NewStatus[Status.PaidPurchase] ||
                    order.status == StatusOrder.NewStatus[Status.PartialPayment] ||
                    order.status == StatusOrder.NewStatus[Status.FullyPaid];
                var isDeliveredToClient = order.status == StatusOrder.NewStatus[Status.DeliveredToClient];

                if (isAccountsPayableStatus && !isDeliveredToClient)
                {
                    if (newPaid <= 0)
                        order.status = StatusOrder.NewStatus[Status.ConfirmSale];
                    else if (newPaid >= totalPrice)
                        order.status = StatusOrder.NewStatus[Status.FullyPaid];
                    else
                        order.status = StatusOrder.NewStatus[Status.PartialPayment];
                }
            }

            await _context.SaveChangesAsync();
            return new SuccessResponse(true, 200, SuccessMessages.OrdersUpdated);
        }
    }
}
