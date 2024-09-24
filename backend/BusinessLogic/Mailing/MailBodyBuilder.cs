using Common.Helpers;
using MimeKit;
using MimeKit.Utils;
using System.Reflection;
using System.Text;
using System.Web;

namespace DomainLayer.BusinessLogic.Mailing
{
    /// <summary>
    /// Builder class for E-Mail BodyBuilder
    /// </summary>
    public class MailBodyBuilder
    {
        private readonly int maxLineWidth = 80;

        private readonly StringBuilder htmlBuilder = new();
        private readonly BodyBuilder bodyBuilder = new();

        /// <summary>
        /// Initializes new instance of <see cref="MailBodyBuilder"/>
        /// </summary>
        /// <param name="mainHeading">The main heading of the mail</param>
        public MailBodyBuilder(string mainHeading)
        {
            this.htmlBuilder
                .AppendLine("<div class=\"container\">");

            this.AddStyling();
            this.AddHeader(mainHeading);

            this.htmlBuilder
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
            this.bodyBuilder.TextBody = HtmlHelper.LimitLineLength(
                HtmlHelper.HtmlToPlainText(this.bodyBuilder.HtmlBody),
                this.maxLineWidth);

            return this.bodyBuilder;
        }

        /// <summary>
        /// Adds a heading
        /// </summary>
        /// <param name="content">The text content</param>
        /// <param name="level">The heading level</param>
        public void AddHeading(string content, int level = 1)
        {
            string cleanContent = this.Sanitize(content);

            this.htmlBuilder
                .AppendLine($"<h{level}>{cleanContent}</h{level}>");
        }

        /// <summary>
        /// Adds a paragraph
        /// </summary>
        /// <param name="content">The text content</param>
        public void AddParagraph(string content)
        {
            string cleanContent = this.Sanitize(content);

            this.htmlBuilder
                .AppendLine($"<p>{cleanContent}</p>");
        }

        /// <summary>
        /// Adds an unordered list
        /// </summary>
        /// <param name="items">The items in the list</param>
        public void AddUnorderedList(IEnumerable<string> items)
        {
            this.htmlBuilder.AppendLine("<ul>");

            foreach (string item in items)
            {
                string cleanItem = this.Sanitize(item);
                this.htmlBuilder.AppendLine($"<li>{cleanItem}</li>");
            }

            this.htmlBuilder.AppendLine("</ul>");
        }

        /// <summary>
        /// Adds a card-style element
        /// </summary>
        /// <param name="content">The text content</param>
        public void AddCard(string content) => this.AddCard([content]);

        /// <summary>
        /// Adds a card-style element
        /// </summary>
        /// <param name="paragraphs">The paragraphs in the card</param>
        public void AddCard(string[] paragraphs)
        {
            this.htmlBuilder
                .AppendLine("<div class=\"card\">");

            foreach (string paragraph in paragraphs)
            {
                this.AddParagraph(paragraph);
            }

            this.htmlBuilder
                .AppendLine("</div>");
        }

        private void AddStyling()
        {
            this.htmlBuilder.AppendLine(
                    $@"
<style>
    html, body {{
        padding: 0;
        margin: 0;
        font-family: sans-serif;
    }}

    .container {{
        width: 100%;
        height: 100%;

        display: grid;
        grid-template-rows: min-content, auto;
    }}

    header {{
        grid-row: 1;
        display: flex;

        justify-content: space-between;
        align-items: center;
        padding-top: 0.5rem;
        padding-bottom: 0.5rem; 

        background-color: #409EFF;
    }}

    header > h1 {{
        padding-inline: 5ch;
    }}

    header > .logo {{
        padding-inline: 5ch;
    }}

    .logo {{
        width: 100px;
        height: 100px;
    }}

    .content {{
        grid-row: 2;
        margin-inline: 10ch;
        max-width: {this.maxLineWidth}ch;
    }}

    .card {{
        padding: 20px;
        background-color: #e2e2e2;
    }}
</style>
                ");
        }

        private void AddHeader(string heading)
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
                .AppendLine("<header>")
                .AppendLine($"<h1>{heading}</h1>")
                .AppendLine($@"<img class='logo' src='cid:{logo.ContentId}'/>")
                .AppendLine("</header>");
        }

        private string Sanitize(string input)
        {
            //ReadOnlySpan<byte> inputSpan = Encoding.UTF8.GetBytes(input);
            //Span<byte> output = [];
            //HtmlEncoder.Default.EncodeUtf8(inputSpan, output, out _, out _);

            //return Encoding.UTF8.GetString(output);
            return HttpUtility.HtmlEncode(input);
        }
    }
}
