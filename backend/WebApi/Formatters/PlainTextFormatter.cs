using Microsoft.AspNetCore.Mvc.Formatters;
using System.Text;

namespace Application.WebApi.Formatters
{
    /// <summary>
    /// Formatter for plain text formatation.
    /// </summary>
    public class PlainTextFormatter : TextInputFormatter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlainTextFormatter"/> class.
        /// </summary>
        public PlainTextFormatter()
        {
            this.SupportedMediaTypes.Add(new Microsoft.Net.Http.Headers.MediaTypeHeaderValue("text/plain"));
            this.SupportedEncodings.Add(Encoding.UTF8);
        }

        /// <inheritdoc/>
        public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context, Encoding encoding)
        {
            using var reader = new StreamReader(context.HttpContext.Request.Body, encoding);
            var plainText = await reader.ReadToEndAsync();

            return await InputFormatterResult.SuccessAsync(plainText);
        }

        /// <inheritdoc/>
        protected override bool CanReadType(Type type) => type == typeof(string);
    }
}
