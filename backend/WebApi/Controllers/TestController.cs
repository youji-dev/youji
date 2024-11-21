using DomainLayer.BusinessLogic.Mailing;
using MailKit;
using Microsoft.AspNetCore.Mvc;
using PersistenceLayer.DataAccess.Entities;

namespace Application.WebApi.Controllers
{
    // For Development. remove before merge


    [Route("mail")]
    public class TestController : ControllerBase
    {
        [HttpGet("ticketChanged")]
        public IActionResult SendTicketChanged([FromServices] MailingService mailService)
        {
            Ticket ticket = new()
            {
                Id = Guid.NewGuid(),
                Title = "Kaputter Stuhl",
                Priority = new() { Id = Guid.NewGuid(), Name = "Niedrig", Value = 2 },
                State = new State() { Id = Guid.NewGuid(), Name = "Neu", Color = "hui" },
                Description = "<b>Some description</b>",
                Author = "Author",
                Object = "Stuhl in der Ecke",
            };

            Ticket changedTicket = new()
            {
                Id = ticket.Id,
                Title = "Kaputter Tisch",
                State = new State() { Id = Guid.NewGuid(), Name = "Aktiv", Color = "hui" },
                Description = "Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. \r\n\r\nDuis autem vel eum iriure dolor in hendrerit in vulputate velit esse molestie consequat, vel illum dolore eu feugiat nulla facilisis at vero eros et accumsan et iusto odio dignissim qui blandit praesent luptatum zzril delenit augue duis dolore te feugait nulla facilisi. Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diam nonummy nibh euismod tincidunt ut laoreet dolore magna aliquam erat volutpat. ",
                Author = "Author",
                Object = "Tisch in der Ecke",
                Room = "200",
                Priority = new() { Id = Guid.NewGuid(), Name = "Hoch", Value = 1 },
                Comments = [new() { TicketId = ticket.Id, Id = Guid.NewGuid(), Author = "Comment author", Content = "Hier ist ein toller Kommentar", CreationDate = DateTime.Now}, new() { TicketId = ticket.Id, Id = Guid.NewGuid(), Author = "Comment author", Content = "Hier ist ein toller Kommentar", CreationDate = DateTime.Now }],
                Attachments = [new() { TicketId = ticket.Id, Id = Guid.NewGuid(), Name = "NewFile.pdf", Binary = [], FileType = "pdf" }, new() { TicketId = ticket.Id, Id = Guid.NewGuid(), Name = "NewFile.pdf", Binary = [], FileType = "pdf" }],
            };

            var mailBody = mailService.GenerateTicketChangedMail(changedTicket, ticket);

            mailService.Send(new("Test", "Test@user.com"), "Some subject", mailBody);

            return this.Ok();
        }

        [HttpGet("newAttachment")]
        public IActionResult SendNewAttachment([FromServices] MailingService mailingService)
        {
            Ticket ticket = new()
            {
                Id = Guid.NewGuid(),
                Title = "Kaputter Stuhl",
                State = new State() { Id = Guid.NewGuid(), Name = "Some state", Color = "hui" },
                Description = "<b>Some description</b>",
                Author = "Author",
                Object = "Some object",
                Priority = new() { Id = Guid.NewGuid(), Name = "High", Value = 1 },
            };

            TicketAttachment newAttachment = new()
            {
                Id = Guid.NewGuid(),
                Name = "Stuhl.png",
                FileType = "png",
                Binary = [],
                TicketId = ticket.Id,
                Ticket = ticket,
            };

            var mailBody = mailingService.GenerateNewTicketAttachmentMail(newAttachment);

            mailingService.Send(new("Test", "Test@user.com"), "Some subject", mailBody);

            return this.Ok();
        }

        [HttpGet("newComment")]
        public IActionResult SendNewComment([FromServices] MailingService mailingService)
        {
            Ticket ticket = new()
            {
                Id = Guid.NewGuid(),
                Title = "Kaputter Stuhl",
                State = new State() { Id = Guid.NewGuid(), Name = "Some state", Color = "hui" },
                Description = "<b>Some description</b>",
                Author = "Author",
                Object = "Some object",
                Priority = new() { Id = Guid.NewGuid(), Name = "High", Value = 1 },
            };

            TicketComment newComment = new()
            {
                Id = Guid.NewGuid(),
                Author = "Max Muster",
                CreationDate = DateTime.Now,
                Content = "Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. \r\n\r\nDuis autem vel eum iriure dolor in hendrerit in vulputate velit esse molestie consequat, vel illum dolore eu feugiat nulla facilisis at vero eros et accumsan et iusto odio dignissim qui blandit praesent luptatum zzril delenit augue duis dolore te feugait nulla facilisi. Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diam nonummy nibh euismod tincidunt ut laoreet dolore magna aliquam erat volutpat. ",
                TicketId = ticket.Id,
                Ticket = ticket,
            };

            var mailBody = mailingService.GenerateNewTicketCommentMail(newComment);

            mailingService.Send(new("Test", "Test@user.com"), "Some subject", mailBody);

            return this.Ok();
        }
    }
}
