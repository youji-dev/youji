using I18N.DotNet;
using QuestPDF.Fluent;

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
        /// <param name="localizer">The localizer to use for the export</param>
        /// <returns>The generated pdf as binary</returns>
        public byte[] Export(TicketExportModel model, Localizer localizer)
        {
            TicketExportDocument document = new(model, localizer);

            return Document.Create(document.Compose).GeneratePdf();
        }
    }
}
