using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.BusinessLogic.Mailing
{
    public struct MailRecipient
    {
        public MailboxAddress Address { get; init;  }

        public string? PreferredLcid { get; init; }
    }
}
