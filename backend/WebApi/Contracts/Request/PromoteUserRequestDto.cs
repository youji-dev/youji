namespace Application.WebApi.Contracts.Request
{
    /// <summary>
    /// Data transfer object for user promotion requests
    /// </summary>
    public class PromoteUserRequestDto
    {
        /// <summary>
        /// Token used to verify the promotion request
        /// </summary>
        public string PromotionToken { get; set; } = string.Empty;
    }
}