using QuestPDF.Fluent;

namespace DomainLayer.BusinessLogic.PDF
{
    public class PDFExportSerivce
    {
        public byte[] Export(TicketExportModel model)
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
