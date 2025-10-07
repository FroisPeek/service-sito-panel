using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ServiceSitoPanel.src.dtos.solicitations;
using ServiceSitoPanel.src.model;

namespace ServiceSitoPanel.src.interfaces
{
    public interface ISolicitationsService
    {
        Task<IResponses> GetAllSolicitations();
        Task<IResponses> RegistreInSolicitation([FromBody] RegistreInSolicitationDto dto);
    }
}