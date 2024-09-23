using MimeKit;
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

        /// <summary>
        /// Initializes new instance of <see cref="MailBodyBuilder"/>
        /// </summary>
        public MailBodyBuilder()
        {
            this.htmlBuilder
                .AppendLine("<div class=\"container\">")
                .AppendLine(
                    $@"
<style>
    .container {{
        max-width: {this.maxLineWidth}ch
    }}
</style>
                ");
        }

        /// <summary>
        /// Completes the mail body
        /// </summary>
        /// <returns>The generated body</returns>
        public BodyBuilder Complete()
        {
            this.htmlBuilder.AppendLine("</div>");

            BodyBuilder builder = new()
            {
                HtmlBody = this.htmlBuilder.ToString(),
                TextBody = this.plainTextBuilder.ToString(),
            };

            return builder;
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
