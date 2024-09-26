using DomainLayer.BusinessLogic.Mailing;
using Microsoft.AspNetCore.Mvc;
using PersistenceLayer.DataAccess.Entities;

namespace Application.WebApi.Controllers
{
    // For Development. remove before merge


    [Route("mail")]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public IActionResult SendMail([FromServices] MailingService mailService)
        {
            Ticket ticket = new()
            {
                Id = Guid.NewGuid(),
                Title = "Some title",
                State = new State() { Id = Guid.NewGuid(), Name = "Some state", Color = "hui" },
                Description = "<b>Some description</b>",
                Author = "Author",
                Object = "Some object",
            };

            Ticket changedTicket = new()
            {
                Id = ticket.Id,
                Title = "Some other title",
                State = ticket.State,
                Description = "Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. \r\n\r\nDuis autem vel eum iriure dolor in hendrerit in vulputate velit esse molestie consequat, vel illum dolore eu feugiat nulla facilisis at vero eros et accumsan et iusto odio dignissim qui blandit praesent luptatum zzril delenit augue duis dolore te feugait nulla facilisi. Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diam nonummy nibh euismod tincidunt ut laoreet dolore magna aliquam erat volutpat. ",
                Author = "Author",
                Object = "Some object",
                Room = "New room",
                Comments = [new() { Id = Guid.NewGuid(), Author = "Comment author", Content = "Hier ist ein toller Kommentar", CreationDate = DateTime.Now}, new() { Id = Guid.NewGuid(), Author = "Comment author", Content = "Hier ist ein toller Kommentar", CreationDate = DateTime.Now }],
                Attachments = [new() { Id = Guid.NewGuid(), Name = "NewFile.pdf", Binary = [], FileType = "pdf" }, new() { Id = Guid.NewGuid(), Name = "NewFile.pdf", Binary = [], FileType = "pdf" }],
            };

            var mailBody = mailService.GenerateTicketChangedMail(changedTicket, ticket);

            mailService.Send(new("Test", "Test@user.com"), "Some subject", mailBody);

            return this.Ok();
        }
    }
}
