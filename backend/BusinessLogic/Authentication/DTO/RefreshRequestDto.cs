namespace DomainLayer.BusinessLogic.Authentication.DTO
{
    /// <summary>
    /// DTO used to parse incoming data for refresh route in the AuthController
    /// </summary>
    public class RefreshRequestDto
    {
        /// <summary>
        /// Refresh token used to create a new token pair
        /// </summary>
        public required string RefreshToken { get; set; }
    }
}