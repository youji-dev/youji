namespace DomainLayer.BusinessLogic.Mailing.Models
{
    /// <summary>
    /// Base mail model
    /// </summary>
    public abstract record MailModel
    {
        /// <summary>
        /// Filename of the corresponding Razor-Template
        /// </summary>
        public abstract string TemplateName { get; }

        /// <summary>
        /// The main mail title
        /// </summary>
        public required string MailTitle { get; set; }

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
