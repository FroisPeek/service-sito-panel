using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ServiceSitoPanel.src.constants;
using ServiceSitoPanel.src.dtos.orders;
using ServiceSitoPanel.src.enums;
using ServiceSitoPanel.src.functions;
using ServiceSitoPanel.src.helpers;
using ServiceSitoPanel.src.model;

namespace ServiceSitoPanel.src.mappers
{
    public static class OrderMapper
    {
        public static Orders ToCreateOrder(this CreateOrderDto order, int tenant_id)
        {
            return new Orders
            {
                client = order.client,
                brand = order.brand,
                code = order.code,
                description = order.description,
                size = order.size,
                amount = order.amount,
                cost_price = order.cost_price,
                sale_price = order.sale_price,
                total_price = order.amount * order.sale_price,
                status = StatusHelper.CompraPendente,
                date_creation_order = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, HandleFunctions.GetTimeZone()),
                tenant_id = tenant_id,
                date_order = null,
            };
        }

        public static void UpdateOrderStatusByValue(this Orders order, int value)
        {
            order.status = HandleFunctions.SelectStatus(value);
            var now = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, HandleFunctions.GetTimeZone());

            switch (value)
            {
                case 1:
                    break;

                case 2:
                    break;

                case 3:
                    break;

                case 4:
                    order.date_order = now;
                    break;

                case 5:
                    order.date_purchase_order = now;
                    break;

                default:
                    throw new ArgumentException(ErrorMessages.InternalServerError, nameof(value));
            }
        }
    }
}