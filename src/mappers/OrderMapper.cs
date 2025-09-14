using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ServiceSitoPanel.src.constants;
using ServiceSitoPanel.src.dtos.orders;
using ServiceSitoPanel.src.functions;
using ServiceSitoPanel.src.model;

namespace ServiceSitoPanel.src.mappers
{
    public static class OrderMapper
    {
        public static ReadOrdersDto ToReadAllOrders(this Orders orders)
        {
            return new ReadOrdersDto
            {
                id = orders.id,
                code = orders.code,
                description = orders.description,
                size = orders.size,
                amount = orders.amount,
                cost_price = orders.cost_price,
                sale_price = orders.sale_price ?? 0,
                total_price = orders.total_price ?? 0,
                paid_price = orders.price_paid ?? 0,
                status = orders.status,
                date_creation_order = orders.date_creation_order,
                tenant_id = orders.tenant_id,
                brand = orders.brand,
                date_order = orders.date_order,
                date_purchase_order = orders.date_purchase_order,
                status_conference = orders.status_conference,
                date_conference = orders.date_conference,
                client_infos = orders.ClientJoin != null
                ? new ClientDto
                {
                    client_id = orders.ClientJoin.id,
                    client_name = orders.ClientJoin.name
                }
                : null
            };
        }

        public static Orders ToCreateOrder(this CreateOrderDto order, int tenant_id, int client_id)
        {
            return new Orders
            {
                client = client_id,
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
            order.status = value > 5 ? order.status : HandleFunctions.SelectStatus(value);
            var now = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, HandleFunctions.GetTimeZone());

            switch (value)
            {
                case 1:
                    break;

                case 2:
                    break;

                case 3:
                    order.client = null;
                    order.sale_price = null;
                    order.total_price = null;
                    break;

                case 4:
                    order.date_order = now;
                    break;

                case 5:
                    order.date_purchase_order = now;
                    order.status_conference = StatusOrder.NewStatus[Status.ToCheck];
                    break;

                case 8:
                    order.status_conference = StatusOrder.NewStatus[Status.Checked];
                    order.date_conference = now;
                    break;

                default:
                    throw new ArgumentException(ErrorMessages.InternalServerError, nameof(value));
            }
        }

        public static void MapToOrder(this NewClientInOrderDto dto, Orders order, int tenant_id, int client_id)
        {
            order.client = client_id;
            order.status = StatusOrder.NewStatus[Status.PaidPurchase];
            order.tenant_id = tenant_id;
            order.sale_price = dto.sale_price;
            order.total_price = dto.total_price;
        }

    }
}