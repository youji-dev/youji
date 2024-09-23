using I18N.DotNet;
using MimeKit;
using PersistenceLayer.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.BusinessLogic.Mailing
{
    public class MailBodyBuilder
    {
        private readonly int maxLineWidth = 80;

        private readonly StringBuilder htmlBuilder = new();
        private readonly StringBuilder plainTextBuilder = new();

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

        public BodyBuilder Complete()
        {
            this.htmlBuilder.AppendLine("</div>");

            BodyBuilder builder = new();

            builder.HtmlBody = this.htmlBuilder.ToString();
            builder.TextBody = this.plainTextBuilder.ToString();

            return builder;
        }

        public void AddHeading(string content, int level = 1)
        {
            this.htmlBuilder
                .AppendLine($"<h{level}>{content}</h{level}>");

            this.plainTextBuilder
                .AppendLine(this.BreakStringToMaxLineWidth(content))
                .AppendLine();
        }

        public void AddParagraph(string content)
        {
            this.htmlBuilder
                .AppendLine($"<p>{content}</p>");

            this.plainTextBuilder
                .AppendLine("\t" + this.BreakStringToMaxLineWidth(content))
                .AppendLine();
        }

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

        private string BreakStringToMaxLineWidth(string s)
        {
            StringBuilder limitedString = new();

            do
            {
                int charactersInLine = Math.Min(s.Length, this.maxLineWidth);
                string line = string.Join(string.Empty, s.Take(charactersInLine));

                limitedString.AppendLine(line);
                s = s.Remove(0, charactersInLine);
            }
            while (s.Length > 0);

            return limitedString.ToString();
        }
    }
}
