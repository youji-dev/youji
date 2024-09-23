using MimeKit;
using MimeKit.Utils;
using System.Reflection;
using System.Text;

namespace DomainLayer.BusinessLogic.Mailing
{
    /// <summary>
    /// Builder class for E-Mail BodyBuilder
    /// </summary>
    public class MailBodyBuilder
    {
        private readonly int maxLineWidth = 80;

        private readonly StringBuilder htmlBuilder = new();
        private readonly StringBuilder plainTextBuilder = new();
        private readonly BodyBuilder bodyBuilder = new();

        /// <summary>
        /// Initializes new instance of <see cref="MailBodyBuilder"/>
        /// </summary>
        public MailBodyBuilder()
        {
            this.AddStyling();
            this.AddBranding();

            this.htmlBuilder
                .AppendLine("<div class\"container\">")
                .AppendLine("<div class=\"content\">");
        }

        /// <summary>
        /// Completes the mail body
        /// </summary>
        /// <returns>The generated body</returns>
        public BodyBuilder Complete()
        {
            this.htmlBuilder
                .AppendLine("</div>")
                .AppendLine("</div>");

            this.bodyBuilder.HtmlBody = this.htmlBuilder.ToString();
            this.bodyBuilder.TextBody = this.plainTextBuilder.ToString();

            return this.bodyBuilder;
        }

        /// <summary>
        /// Adds a heading
        /// </summary>
        /// <param name="content">The text content</param>
        /// <param name="level">The heading level</param>
        public void AddHeading(string content, int level = 1)
        {
            this.htmlBuilder
                .AppendLine($"<h{level}>{content}</h{level}>");

            this.plainTextBuilder
                .AppendLine(this.FormatForPlainText(content))
                .AppendLine();
        }

        /// <summary>
        /// Adds a paragraph
        /// </summary>
        /// <param name="content">The text content</param>
        public void AddParagraph(string content)
        {
            this.htmlBuilder
                .AppendLine($"<p>{content}</p>");

            this.plainTextBuilder
                .AppendLine("\t" + this.FormatForPlainText(content))
                .AppendLine();
        }

        /// <summary>
        /// Adds an unordered list
        /// </summary>
        /// <param name="items">The items in the list</param>
        public void AddUnorderedList(IEnumerable<string> items)
        {
            this.htmlBuilder.AppendLine("<ul>");
            this.plainTextBuilder.AppendLine();

            foreach (string item in items)
            {
                this.htmlBuilder.AppendLine($"<li>{item}</li>");
                this.plainTextBuilder.AppendLine($"\t- {item}");
            }

            this.htmlBuilder.AppendLine("</ul>");
            this.plainTextBuilder.AppendLine();
        }

        private void AddStyling()
        {
            this.htmlBuilder.AppendLine(
                    $@"
<style>
    .container {{
        position: relative;
        width: 100%;
        height: 100%;
    }}

    .logo {{
        position: absolute;
        top: 10;
        right: 10;
        width: 100px;
        height: 100px;
    }}

    .content {{
        max-width: {this.maxLineWidth}ch;
        font-family: sans-serif;
    }}

    h1, h2, h3, h4, h5, h6, p, li {{
        font-family: inherit;
    }}
</style>
                ");
        }

        private void AddBranding()
        {
            Assembly? assembly = Assembly.GetEntryAssembly();
            if (assembly is null)
                return;

            string logoResourceName = assembly
                .GetManifestResourceNames()
                .Single(name => name.EndsWith("Logo.svg"));

            var logo = this.bodyBuilder.LinkedResources.Add(
                "Logo.svg",
                assembly.GetManifestResourceStream(logoResourceName));

            logo.ContentId = MimeUtils.GenerateMessageId();

            this.htmlBuilder
                .AppendLine($@"<img class='logo' src='cid:{logo.ContentId}'/>");
        }

        private string FormatForPlainText(string s, bool indent = false)
        {
            StringBuilder limitedString = new();

            do
            {
                int charactersInLine = Math.Min(s.Length, this.maxLineWidth);
                string line = string.Join(string.Empty, s.Take(charactersInLine));

                limitedString.AppendLine($"{(indent ? "\t" : string.Empty)}{line}");
                s = s.Remove(0, charactersInLine);
            }
            while (s.Length > 0);

            return limitedString.ToString();
        }
    }
}
