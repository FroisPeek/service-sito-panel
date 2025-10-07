using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ServiceSitoPanel.src.dtos.solicitations;
using ServiceSitoPanel.src.interfaces;
using ServiceSitoPanel.src.services;

namespace ServiceSitoPanel.src.controllers
{
    [ApiController]
    [Route("api/solicitations")]
    public class SolicitationsController : ControllerBase
    {
        private readonly ISolicitationsService _solicitations;
        public SolicitationsController(ISolicitationsService solicitations)
        {
            _solicitations = solicitations;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetProfiles()
        {
            var result = await _solicitations.GetAllSolicitations();

            if (!result.Flag) ResponseHelper.HandleError(this, result);

            return Ok(result);
        }

        [Authorize]
        [HttpPost("add-in-solicitation")]
        public async Task<IActionResult> AddInSolicitation([FromBody] RegistreInSolicitationDto dto)
        {
            var result = await _solicitations.RegistreInSolicitation(dto);

            if (!result.Flag) ResponseHelper.HandleError(this, result);

            return Ok(result);
        }
    }
}