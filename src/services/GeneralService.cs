using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using leapcert_back.src.context;
using Microsoft.EntityFrameworkCore;
using ServiceSitoPanel.src.interfaces;
using ServiceSitoPanel.src.model;
using static ServiceSitoPanel.src.responses.ResponseFactory;

namespace ServiceSitoPanel.src.services
{
    public class GeneralService : IGeneralService
    {
        private readonly ApplicationDbContext _context;

        public GeneralService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IResponses> GetAllProfiles()
        {
            var profiles = await _context.profiles.ToListAsync();

            if (profiles.Count == 0)
                return new ErrorResponse(false, 404, "Nenhum usuario encontrado");

            return new SuccessResponse<IEnumerable<Profile>>(true, 200, "Sucesso ao retornar usuarios", profiles);
        }
    }
}