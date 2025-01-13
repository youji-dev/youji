using Common.Extensions;
using Microsoft.Extensions.Configuration;
using PersistenceLayer.DataAccess.Repositories;
using Quartz;

namespace DomainLayer.BusinessLogic.Jobs
{
    /// <summary>
    /// Represents a job that deletes expired refresh token after a given time.
    /// </summary>
    /// <param name="tokenRepo">Instance of <see cref="RefreshTokenRepository"/></param>
    public class PurgeExpiredRefreshTokenJob(RefreshTokenRepository tokenRepo, IConfiguration configuration) : IJob
    {
        /// <inheritdoc/>
        public async Task Execute(IJobExecutionContext context)
        {
            var hasSessionLifeTimeValue = int.TryParse(configuration.GetValueOrThrow("SessionLifeTime"), out int sessionLifeTime);

            if (!hasSessionLifeTimeValue)
                throw new InvalidOperationException("There is no value in configuration for 'SessionLifeTime'");

            var deletableTokens = tokenRepo
                .GetAll()
                .Where(token => DateTime.UtcNow.AddMinutes(-sessionLifeTime) > token.CreationDateTime)
                .ToArray();

            foreach (var token in deletableTokens)
                await tokenRepo.DeleteAsync(token);
        }
    }
}
