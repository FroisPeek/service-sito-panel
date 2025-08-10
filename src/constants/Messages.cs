using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceSitoPanel.src.constants
{
    public static class ErrorMessages
    {
        public const string NoOrdersFound = "Nenhum pedido encontrado.";
        public const string MissingOrderFields = "Preciso informar os campos do pedido.";
        public const string MissingOrderCodes = "Código dos pedidos não registrados.";
        public const string SomeOrdersNotFound = "Alguns pedidos informados não foram encontrados em nossa base.";
        public const string InternalServerError = "Ocorreu um erro interno no servidor.";
    }

    public static class SuccessMessages
    {
        public const string OrdersRetrieved = "Pedidos retornados com sucesso.";
        public const string OrderCreated = "Pedido cadastrado com sucesso.";
        public const string OrdersUpdated = "Pedidos atualizados com sucesso.";
    }
}