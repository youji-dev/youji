using I18N.DotNet;
using QuestPDF.Fluent;
using System.Globalization;
using System.Reflection;
using Common.Extensions;

namespace DomainLayer.BusinessLogic.PDF
{
    /// <summary>
    /// Provides functions relating to pdf exports
    /// </summary>
    public class ExportService
    {
        /// <summary>
        /// <para>Initializes new instance of <see cref="ExportService"/></para>
        /// <para>Performs required configurations for QuestPDF</para>
        /// </summary>
        public ExportService()
        {
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;
        }

        /// <summary>
        /// Export a ticket to pdf
        /// </summary>
        /// <param name="model">The ticket model to use for the document</param>
        /// <param name="lang">The target language for the export</param>
        /// <returns>The generated pdf as binary</returns>
        public byte[] Export(TicketExportModel model, string lang)
        {
            Localizer localizer = new();
            using var localizerResourceStream = Assembly.GetExecutingAssembly().GetResource("PDF.I18N.xml");
            if (localizerResourceStream is not null)
            {
                localizer.LoadXML(localizerResourceStream, CultureInfo.GetCultureInfo(lang ?? CultureInfo.CurrentCulture.Name));
            }

            TicketExportDocument document = new(model, localizer);

            return Document.Create(document.Compose).GeneratePdf();
        }
    }
}
