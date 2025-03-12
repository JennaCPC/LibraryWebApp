using Library.Application.Interfaces;
using MimeKit;

namespace Library.Server.Common
{
    public class EmailTemplateProvider(IWebHostEnvironment env) : IEmailTemplateProvider
    {
        private string GetFilePath(string fileName)
        {
            return env.WebRootPath
                + Path.DirectorySeparatorChar.ToString()
                + "Templates"
                + Path.DirectorySeparatorChar.ToString()
                + "EmailTemplate"
                + Path.DirectorySeparatorChar.ToString()
                + fileName;
        }

        public string EmailConfirmation(string link)
        {
            var builder = new BodyBuilder();

            using StreamReader reader = File.OpenText(GetFilePath("Confirm_Email.html"));
            builder.HtmlBody = reader.ReadToEnd();

            return string.Format(builder.HtmlBody, link);
        }

        public string PasswordReset(string link)
        {
            var builder = new BodyBuilder();

            using StreamReader reader = File.OpenText(GetFilePath("Reset_Password.html"));
            builder.HtmlBody = reader.ReadToEnd();

            return string.Format(builder.HtmlBody, link);
        }        
    }
}
