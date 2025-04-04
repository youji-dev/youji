using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.BusinessLogic.Mailing
{
    /// <summary>
    /// Contains values from the appconfig used during MailGeneration
    /// </summary>
    /// <param name="FrontendTicketBaseUrl">Base uri of the frontend ticket route</param>
    public record MailGenConfigurationDto(string FrontendTicketBaseUrl);
}
