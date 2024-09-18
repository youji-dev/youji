using QuestPDF.Fluent;

namespace DomainLayer.BusinessLogic.PDF
{
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

        public byte[] ExportAsPDF(TicketExportModel model)
        {
            TicketExportDocument document = new (model);

            return Document.Create(document.Compose).GeneratePdf();
        }

        /// <summary>
        /// Debug method
        /// </summary>
        /// <param name="model"></param>
        public void Show(TicketExportModel model)
        {
            TicketExportDocument document = new(model);

            Document.Create(document.Compose).GeneratePdfAndShow();
        }
    }
}
