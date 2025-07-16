using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ServiceSitoPanel.src.interfaces;

namespace ServiceSitoPanel.src.controllers
{
    [Route("api/general")]
    public class GeneralController : ControllerBase
    {
        private readonly IGeneralService _general;
        public GeneralController(IGeneralService general)
        {
            _general = general;
        }

        [HttpGet("getProfiles")]
        public async Task<IActionResult> GetProfiles()
        {
            var result = await _general.GetAllProfiles();

            if (!result.Flag) ResponseHelper.HandleError(this, result);

            return Ok(result);
        }
    }
}