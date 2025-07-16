using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceSitoPanel.src.interfaces
{
    public interface IResponses
    {
        bool Flag { get; }
        int StatusCode { get; }
        string Message { get; }
    }
}