namespace DomainLayer.BusinessLogic.Mailing.Models
{
    /// <summary>
    /// Base mail model
    /// </summary>
    public record MailModel
    {
        /// <summary>
        /// The main mail title
        /// </summary>
        public required string Title { get; set; }

        /// <summary>
        /// The youji logo src
        /// </summary>
        public string LogoSrc { get; set; } = string.Empty;

        /// <summary>
        /// src for an 'arrow-right' icon
        /// </summary>
        public string ArrowRightIconSrc { get; set; } = string.Empty;

        /// <summary>
        /// src for an 'arrow-down' icon
        /// </summary>
        public string ArrowDownIconSrc { get; set; } = string.Empty;
    }
}
