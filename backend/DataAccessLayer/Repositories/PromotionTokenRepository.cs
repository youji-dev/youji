namespace PersistenceLayer.DataAccess.Repositories
{
    /// <summary>
    /// As singleton injected Service class that is used to store the PromotionToken
    /// </summary>
    public class PromotionTokenRepository
    {
        private string? promotionToken = null;
        private DateTime? tokenCreationTime = null;
        private readonly object lockObject = new object();

        /// <summary>
        /// Gets the stored string value in a thread-safe manner.
        /// </summary>
        /// <returns>A tuple containing the token and when it was created</returns>
        public (string? PromotionToken, DateTime? TokenCreationTime) Get()
        {
            lock (this.lockObject)
            {
                return (this.promotionToken, this.tokenCreationTime);
            }
        }

        /// <summary>
        /// Sets a new string value in a thread-safe manner.
        /// </summary>
        /// <param name="value">The new value to store.</param>
        public void SetToken(string? value)
        {
            lock (this.lockObject)
            {
                this.promotionToken = value;
                this.tokenCreationTime = DateTime.Now;
            }
        }
    }
}