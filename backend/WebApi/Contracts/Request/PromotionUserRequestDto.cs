namespace Application.WebApi.Contracts.Request
{
    /// <summary>
    /// Data transfer object for user promotiopn requests
    /// </summary>
    public class PromotionUserRequestDto
    {
        /// <summary>
        /// Token used to verify the promotion request
        /// </summary>
        public string PromotionToken { get; set; } = string.Empty;
    }
}