using DomainLayer.BusinessLogic.PDF;
using Microsoft.AspNetCore.Mvc;
using PersistenceLayer.DataAccess.Entities;
using PersistenceLayer.DataAccess.Repositories;
using Microsoft.AspNetCore.Authorization;
using System.Globalization;

namespace Application.WebApi.Controllers
{
    /// <summary>
    /// Contains endpoints relating to export functionality
    /// </summary>
    [Authorize]
    public class ExportController : ControllerBase
    {
        /// <summary>
        /// Create an export for the given ticket
        /// </summary>
        /// <param name="ticketRepository">Instance of <see cref="TicketRepository"/></param>
        /// <param name="exportService">Instance of see <see cref="ExportService"/></param>
        /// <param name="ticketId">The Id of the ticket to create an export for</param>
        /// <param name="lang">Language code to localize the export to; if omitted default values are used</param>
        /// <returns>The export for a file download</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [HttpGet("/api/ticket/{ticketId}/export")]
        public async Task<FileContentResult?> ExportTicket(
            [FromServices] TicketRepository ticketRepository,
            [FromServices] ExportService exportService,
            [FromRoute] Guid ticketId,
            [FromQuery] string? lang)
        {
            Ticket? ticket = await ticketRepository.GetAsync(ticketId);

            if (ticket is null)
                return null;

            TicketExportModel model = TicketExportModel.FromTicket(ticket);
            byte[] pdf = exportService.Export(model, lang ?? CultureInfo.CurrentCulture.Name);

            string clampedTicketTitle = ticket.Title[..Math.Min(ticket.Title.Length, 20)];
            FileContentResult result = new(pdf, "application/octet-stream")
            {
                FileDownloadName = $"Export_{clampedTicketTitle.Replace(" ", "-")}_{DateTime.Now:yyyy-MM-dd}_{DateTime.Now.Ticks}.pdf",
            };

            return result;
        }
    }
}
