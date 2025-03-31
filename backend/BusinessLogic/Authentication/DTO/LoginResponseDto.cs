namespace DomainLayer.BusinessLogic.Authentication.DTO
{
    /// <summary>
    /// DTO used to parse outgoing data for login and refresh route in the AuthController
    /// </summary>
    public class LoginResponseDto
    {
        /// <summary>
        /// A JWT AccessToken
        /// </summary>
        public required string AccessToken { get; set; }

        /// <summary>
        /// A RefreshToken
        /// </summary>
        public required string RefreshToken { get; set; }

        /// <summary>
        /// Indicates if the deployment has a registered admin user
        /// </summary>
        public required bool IsSystemReady { get; set; }
    }
}