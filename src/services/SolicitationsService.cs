using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceSitoPanel.src.context;
using ServiceSitoPanel.src.dtos.solicitations;
using ServiceSitoPanel.src.interfaces;
using ServiceSitoPanel.src.model;
using static ServiceSitoPanel.src.responses.ResponseFactory;

namespace ServiceSitoPanel.src.services
{
    public class SolicitationsService : ISolicitationsService
    {
        private readonly ApplicationDbContext _context;
        public SolicitationsService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IResponses> GetAllSolicitations()
        {
            var solicitations = await _context.solicitations.ToListAsync();

            if (solicitations.Count == 0)
                return new ErrorResponse(false, 404, "Nenhuma solicitação encontrada");

            return new SuccessResponse(true, 200, "Solicitações retornadas com sucesso");
        }

        public async Task<IResponses> RegistreInSolicitation([FromBody] RegistreInSolicitationDto dto)
        {
            if (dto.orders == null || dto.orders.Length == 0)
                return new ErrorResponse(false, 400, "Nenhum pedido informado.");

            var validOrders = new List<int>();

            foreach (var orderId in dto.orders)
            {
                var orderExists = await _context.orders.AnyAsync(o => o.id == orderId);

                if (!orderExists)
                    return new ErrorResponse(false, 404, $"Pedido com ID {orderId} não encontrado.");

                validOrders.Add(orderId);
            }

            var currentSolicitation = await _context.solicitations
                .FirstOrDefaultAsync(s => s.id == dto.existingSolicitation);

            Solicitations solicitation;

            if (currentSolicitation == null)
            {
                solicitation = new Solicitations
                {
                    orders = validOrders.ToArray(),
                    status = "Pedido aberto",
                    date_solicitation = DateTime.UtcNow
                };

                _context.solicitations.Add(solicitation);
            }
            else
            {
                var updatedOrders = currentSolicitation.orders?.ToList() ?? new List<int>();

                foreach (var orderId in validOrders)
                {
                    if (!updatedOrders.Contains(orderId))
                        updatedOrders.Add(orderId);
                }

                currentSolicitation.orders = updatedOrders.ToArray();
                currentSolicitation.date_solicitation = DateTime.UtcNow;
                currentSolicitation.status = "Pedido aberto";

                solicitation = currentSolicitation;

                _context.solicitations.Update(currentSolicitation);
            }

            await _context.SaveChangesAsync();

            return new SuccessResponse<Solicitations>(true, 201, "Solicitação registrada com sucesso.", solicitation);
        }

    }
}