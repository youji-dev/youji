namespace DomainLayer.BusinessLogic.Mailing.Models
{
    /// <summary>
    /// Base mail model for resources
    /// </summary>
    internal record MailModel
    {
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
