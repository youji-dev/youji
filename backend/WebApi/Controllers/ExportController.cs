using DomainLayer.BusinessLogic.PDF;
using I18N.DotNet;
using Microsoft.AspNetCore.Mvc;
using PersistenceLayer.DataAccess.Entities;
using PersistenceLayer.DataAccess.Repositories;
using System.Globalization;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Common.Extensions;

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

            Localizer? localizer = null;
            using var localizerResourceStream = Assembly.GetExecutingAssembly().GetResource("Resources.I18N.xml");
            if (!string.IsNullOrWhiteSpace(lang) && localizerResourceStream is not null)
            {
                localizer = new();
                localizer.LoadXML(localizerResourceStream, CultureInfo.GetCultureInfo(lang));
            }

            TicketExportModel model = TicketExportModel.FromTicket(ticket);
            byte[] pdf = exportService.Export(model, localizer);

            FileContentResult result = new(pdf, "application/octet-stream")
            {
                FileDownloadName = $"Export_{ticket.Title[..20].Replace(" ", "-")}_{DateTime.Now:yyyy-MM-dd}_{DateTime.Now.Ticks}.pdf",
            };

            return result;
        }
    }
}
