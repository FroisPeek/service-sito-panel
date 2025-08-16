using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ServiceSitoPanel.src.dtos.orders;
using ServiceSitoPanel.src.model;

namespace ServiceSitoPanel.src.mappers
{
    public static class ClientMapper
    {
        public static Client ToCreateClient(this CreateOrderDto order, int tenant_id)
        {
            return new Client
            {
                name = order.client,
                tenant_id = tenant_id
            };
        }
    }
}