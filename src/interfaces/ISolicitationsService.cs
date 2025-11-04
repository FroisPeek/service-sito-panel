using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ServiceSitoPanel.src.dtos.orders;
using ServiceSitoPanel.src.dtos.solicitations;
using ServiceSitoPanel.src.model;

namespace ServiceSitoPanel.src.interfaces
{
    public interface ISolicitationsService
    {
        Task<IResponses> GetAllSolicitations();
        Task<IResponses> GetSolicitationsWithOrders();
        Task<IResponses> RegistreInSolicitation([FromBody] RegistreInSolicitationDto dto);
        Task<IResponses> SaveOrderAndSolicitation([FromBody] CreateOrderDto[] dto, int? solicitation);
    }
}