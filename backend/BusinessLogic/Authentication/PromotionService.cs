using Common.Enums;
using Microsoft.EntityFrameworkCore;
using PersistenceLayer.DataAccess.Repositories;
using Spectre.Console;

namespace DomainLayer.BusinessLogic.Authentication
{
    /// <summary>
    /// Service used for system setup and configuration
    /// </summary>
    public class PromotionService
    {
        private readonly PromotionTokenRepository promotionTokenRepository;
        private readonly UserRepository userRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="PromotionService"/> class.
        /// </summary>
        /// <param name="userRepository">Instance of <see cref="userRepository"/></param>
        /// <param name="promotionTokenRepository">Singleton instance of <see cref="PromotionTokenRepository"/></param>
        public PromotionService(UserRepository userRepository, PromotionTokenRepository promotionTokenRepository)
        {
            this.userRepository = userRepository;
            this.promotionTokenRepository = promotionTokenRepository;
        }

        /// <summary>
        /// Used on application startup. Generates and logs a promotion token if no admin user exists.
        /// </summary>
        public void AdminBootUpCheck()
        {
            if (this.promotionTokenRepository.Get().PromotionToken != null)
            {
                throw new ApplicationException("Application tried to generate a second promotion token.");
            }

            var doesAdminExists = this.userRepository.GetAll().ToList().Any(user => user.Type.HasFlag(Roles.Admin));
            if (doesAdminExists)
                return;

            var promotionToken = Guid.NewGuid().ToString();
            this.promotionTokenRepository.SetToken(promotionToken);

            var panel = new Panel(promotionToken)
            {
                Header = new PanelHeader("SETUP ADMIN PROMOTION TOKEN"),
            };
            AnsiConsole.MarkupLine("[bold red]This system has no admin user. [/][red]To promote a user to an admin user, use the following token in the frontend.[/]");
            AnsiConsole.MarkupLine("[red]The token is only valid for 10 minutes. You can restart the youji backend to generate a new token.[/]");
            AnsiConsole.Write(panel);
        }

        /// <summary>
        /// Promotes a user to admin if the token is valid.
        /// </summary>
        /// <param name="userId">ID of the user that should be promoted to an admin user</param>
        /// <param name="token">promotion token to validate the request</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task PromoteToAdmin(string userId, string token)
        {
            var (pt, ptCreationTime) = this.promotionTokenRepository.Get();
            if (pt is null || ptCreationTime is null)
                throw new ApplicationException($"Cant promote user '{userId}' to admin. No promotion token was generated");

            if (ptCreationTime.Value.AddMinutes(10) < DateTime.Now)
                throw new ApplicationException($"Cant promote user '{userId}' to admin. Promotion token expired.");

            if (token != pt)
                throw new ApplicationException($"Cant promote user '{userId}' to admin. Invalid promotion token.");

            var doesAdminExists = this.userRepository.GetAll().ToList().Any(user => user.Type.HasFlag(Roles.Admin));
            if (doesAdminExists)
                throw new ApplicationException($"Cant promote user '{userId}' to admin. System already has an admin user.");

            this.promotionTokenRepository.SetToken(null);

            var user = await this.userRepository.Find(user => user.UserId == userId).FirstOrDefaultAsync();
            if (user is null)
                throw new ApplicationException($"Cant promote user '{userId}' to admin. User not found.");

            user.Type = Roles.Admin;
            await this.userRepository.UpdateAsync(user);

            AnsiConsole.MarkupLine($"[green]User '{userId}' was promoted to admin user.[/]");
        }
    }
}