using DomainLayer.BusinessLogic.PDF;
using Microsoft.AspNetCore.Mvc;
using PersistenceLayer.DataAccess.Entities;
using PersistenceLayer.DataAccess.Repositories;

namespace Application.WebApi.Controllers
{
    /// <summary>
    /// Contains endpoints relating to export functionality
    /// </summary>
    public class ExportController : ControllerBase
    {
        /// <summary>
        /// Create an export for the given ticket
        /// </summary>
        /// <param name="ticketRepository">Instance of <see cref="TicketRepository"/></param>
        /// <param name="exportService">Instance of see <see cref="ExportService"/></param>
        /// <param name="ticketId">The Id of the ticket to create an export for</param>
        /// <returns>The export for a file download</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [HttpGet("/api/ticket/{ticketId}/export")]
        public async Task<ActionResult<FileContentResult>> ExportTicket(
            [FromServices] TicketRepository ticketRepository,
            [FromServices] ExportService exportService,
            [FromRoute] Guid ticketId)
        {
            Ticket? ticket = await ticketRepository.GetAsync(ticketId);

            if (ticket is null)
                return this.NotFound(ticketId);

            TicketExportModel model = TicketExportModel.FromTicket(ticket);
            byte[] pdf = exportService.Export(model);

            FileContentResult result = new(pdf, "application/octet-stream")
            {
                FileDownloadName = $"Export_{ticket.Title.Replace(" ", "-")}_{DateTime.Now:yyyy-MM-dd}_{DateTime.Now.Ticks}.pdf",
            };

            return result;
        }
    }
}
