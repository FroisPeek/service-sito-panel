using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ServiceSitoPanel.src.interfaces;

namespace ServiceSitoPanel.src.responses
{
    public class ResponseFactory
    {
        public record SuccessResponse(bool Flag, int StatusCode, string Message) : IResponses;
        public record SuccessResponse<T>(bool Flag, int StatusCode, string Message, T Data) : IResponses;
        public record SuccessResponseWithPagination<T>(
            bool Flag,
            int StatusCode,
            string Message,
            int TotalCount,
            int PageNumber,
            int PageSize,
            int TotalPages,
            IEnumerable<T> Data) : IResponses;
        public record ErrorResponse(bool Flag, int StatusCode, string Message) : IResponses;
    }
}