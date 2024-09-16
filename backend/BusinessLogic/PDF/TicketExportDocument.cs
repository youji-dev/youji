using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace DomainLayer.BusinessLogic.PDF
{
    public class TicketExportDocument(TicketExportModel ticketExportModel) : IDocument
    {
        private readonly TicketExportModel model = ticketExportModel;

        /// <inheritdoc/>
        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        /// <inheritdoc/>
        public DocumentSettings GetSettings() => DocumentSettings.Default;

        /// <inheritdoc/>
        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Header().Element(this.ComposeHeader);
            });
        }

        private void ComposeHeader(IContainer container)
        {
            container.Text(this.model.Title);
        }
    }
}
