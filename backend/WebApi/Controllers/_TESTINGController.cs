using System.Text;
using DomainLayer.BusinessLogic.Mailing;
using Microsoft.AspNetCore.Mvc;
using PersistenceLayer.DataAccess.Entities;

namespace Application.WebApi.Controllers
{
#if DEBUG
    [Route("[controller]")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:Element should begin with upper-case letter", Justification = "The underscore ensures this controller is at the top of swagger")]
    public class _TESTINGController : ControllerBase
    {
        private Priority[] mockPriorities;
        private State[] mockStates;
        private Ticket[] mockTickets;
        private Building[] mockBuildings;
        private TicketAttachment[] mockAttachments;
        private TicketComment[] mockComments;

        private User[] mockUsers;
        private IEnumerable<MailRecipient> mockRecipients;

        public _TESTINGController()
        {
            this.mockUsers = [
                new() { Type = Common.Enums.Roles.Teacher, UserId = "user", Email = "de@test.com", PreferredEmailLcid = "de-DE" },
                new() { Type = Common.Enums.Roles.FacilityManager, UserId = "facilityManager", Email = "en@test.com", PreferredEmailLcid = "en-EN" }];

            this.mockRecipients = MailRecipient.GetCollectionFromUsers(this.mockUsers);

            this.mockPriorities = [
                new()
                {
                    Id = Guid.NewGuid(),
                    Color = "#22ff22",
                    Name = "Low",
                    Value = 1,
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Color = "#ffff00",
                    Name = "Normal",
                    Value = 2,
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Color = "#ff1111",
                    Name = "High",
                    Value = 3,
                }
            ];

            this.mockStates = [
                new()
                {
                    Id = Guid.NewGuid(),
                    Color = "#22ff22",
                    HasAutoPurge = false,
                    Name = "New",
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Color = "#ffff00",
                    HasAutoPurge = false,
                    Name = "Active",
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Color = "#aaaaaa",
                    HasAutoPurge = false,
                    Name = "Done",
                }
            ];

            this.mockBuildings = [
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "Main building",
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "Gym",
                }
            ];

            this.mockTickets = [
                new()
                {
                    Title = "Some new ticket",
                    Author = this.mockUsers[0].UserId,
                    Id = Guid.NewGuid(),
                    CreationDate = DateTime.Now,
                    Description = "Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet.",
                    Priority = this.mockPriorities[0],
                    State = this.mockStates[0],
                    Building = this.mockBuildings[0],
                    Room = "200",
                    Object = "Beamer",
                },
                new()
                {
                    Title = "Some other ticket",
                    Author = this.mockUsers[1].UserId,
                    Id = Guid.NewGuid(),
                    CreationDate = DateTime.Now,
                    Description = "Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam",
                    Priority = this.mockPriorities[1],
                    State = this.mockStates[1],
                    Building = this.mockBuildings[1],
                    Room = "Locker room",
                    Object = "Bench",
                }
            ];

            this.mockComments = [
                new() {
                    Id = Guid.NewGuid(),
                    Author = this.mockUsers[0].UserId,
                    CreationDate = DateTime.Parse("02/05/2024"),
                    TicketId = this.mockTickets[0].Id,
                    Ticket = this.mockTickets[0],
                    Content = "Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam",
                }
            ];

            this.mockAttachments = [
                new()
                {
                    Id = Guid.NewGuid(),
                    FileType = "png",
                    Name = "Lorem.png",
                    TicketId = this.mockTickets[0].Id,
                    Ticket = this.mockTickets[0],
                }
            ];
        }

        [HttpGet("SendAllMails")]
        public async Task SendMails(
            [FromServices] MailingService mailService)
        {
            await mailService.SendManyLocalized(
                this.mockRecipients,
                (localizer) => MailGenerator.GenerateNewTicketMail(this.mockTickets[0], localizer),
                (localizer) => localizer.Localize($"New ticket: '{this.mockTickets[0].Title}'"));

            await mailService.SendManyLocalized(
                this.mockRecipients,
                (localizer) => MailGenerator.GenerateTicketChangedMail(this.mockTickets[1], this.mockTickets[0], localizer),
                (localizer) => localizer.Localize($"Ticket '{this.mockTickets[0].Title}' was changed"));

            await mailService.SendManyLocalized(
                this.mockRecipients,
                (localizer) => MailGenerator.GenerateNewTicketCommentMail(this.mockComments[0], localizer),
                (localizer) => localizer.Localize($"New comment on ticket '{this.mockComments[0].Ticket?.Title}'"));

            await mailService.SendManyLocalized(
                this.mockRecipients,
                (localizer) => MailGenerator.GenerateNewTicketAttachmentMail(this.mockAttachments[0], localizer),
                (localizer) => localizer.Localize($"New attachment on ticket '{this.mockAttachments[0].Ticket?.Title}'"));
        }
    }
#endif
}
