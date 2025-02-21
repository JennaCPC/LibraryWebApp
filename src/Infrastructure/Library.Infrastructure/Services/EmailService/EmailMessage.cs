using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MimeKit;

namespace Library.Infrastructure.Services.EmailService
{
    public class EmailMessage
    {
        public List<MailboxAddress> To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }

        public EmailMessage(IEnumerable<string> to, string subject, string body)
        {
            To = [.. to.Select(x => new MailboxAddress("", x))];
            Subject = subject;
            Body = body;
        }
    }
}
