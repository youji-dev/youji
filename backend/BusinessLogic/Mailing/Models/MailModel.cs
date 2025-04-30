using I18N.DotNet;

namespace DomainLayer.BusinessLogic.Mailing.Models
{
    /// <summary>
    /// Base mail model
    /// </summary>
    public abstract class MailModel
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
        /// Localizer for mail generation
        /// </summary>
        public required Localizer Localizer { get; set; }

        /// <summary>
        /// Contains all properties that are automatically provided by the mail generator; manual values will be overwritten
        /// </summary>
        public ProvidedValues AutoValues { get; } = new();

        /// <summary>
        /// Id of a related ticket
        /// </summary>
        public Guid? RelatedTicketId { get; init; }

        /// <summary>
        /// Represents all properties that are automatically provided by the mail generator
        /// </summary>
        public class ProvidedValues()
        {
            /// <summary>
            /// The youji logo src
            /// </summary>
            public string LogoSrc { get; set; } = string.Empty;

            /// <summary>
            /// src for an 'arrow-right' icon
            /// </summary>
            /// <remarks>
            /// Must be set to `null` to be added to mail by generator
            /// </remarks>
            public string? ArrowRightIconSrc { get; set; } = string.Empty;

            /// <summary>
            /// src for an 'arrow-down' icon
            /// </summary>
            /// <remarks>
            /// Must be set to `null` to be added to mail by generator
            /// </remarks>
            public string? ArrowDownIconSrc { get; set; } = string.Empty;

            /// <summary>
            /// Hyperref for a "Take me there" function
            /// </summary>
            public string GoToHyperref { get; set; } = string.Empty;
        }
    }
}
