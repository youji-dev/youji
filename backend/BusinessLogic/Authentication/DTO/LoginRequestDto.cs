namespace DomainLayer.BusinessLogic.Authentication.DTO
{
    /// <summary>
    /// DTO used to parse incoming data for login route in the AuthController
    /// </summary>
    public class LoginRequestDto
    {
        /// <summary>
        /// Username of the user
        /// </summary>
        public required string Username { get; set; }

        /// <summary>
        /// Clear text password
        /// </summary>
        public required string Password { get; set; }
    }
}