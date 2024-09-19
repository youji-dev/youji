using DomainLayer.BusinessLogic.PDF;
using Microsoft.AspNetCore.Mvc;
using PersistenceLayer.DataAccess.Entities;
using PersistenceLayer.DataAccess.Repositories;

namespace Application.WebApi.Controllers
{
    public class ExportController : ControllerBase
    {
        [HttpGet("/api/ticket/{ticketId}/export")]
        public async Task<IActionResult> ExportTicket(
            [FromServices] TicketRepository ticketRepository,
            [FromServices] ExportService exportService,
            [FromRoute] Guid ticketId)
        {
            Ticket? ticket = await ticketRepository.GetAsync(ticketId);

            if (ticket is null)
                return this.NotFound(ticketId);

            TicketExportModel model = TicketExportModel.FromTicket(ticket);
            byte[] pdf = exportService.ExportAsPDF(model);

            FileContentResult result = new (pdf, "application/octet-stream")
            {
                FileDownloadName = $"Export_{ticket.Title.Replace(" ", "-")}_{DateTime.Now:yyyy-MM-dd}_{DateTime.Now.Ticks}.pdf",
            };

            return result;
        }
    }
}
