using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using System.Globalization;

namespace DomainLayer.BusinessLogic.PDF
{
    public class TicketExportDocument(TicketExportModel ticketExportModel) : IDocument
    {
        private readonly TicketExportModel model = ticketExportModel;

        private readonly int borderThickness = 1;
        private readonly int padding = 20;

        private readonly int smallFont = 10;
        private readonly int largeFont = 20;

        /// <inheritdoc/>
        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        /// <inheritdoc/>
        public DocumentSettings GetSettings() => DocumentSettings.Default;

        /// <inheritdoc/>
        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Margin(this.padding);

                page.Header().Element(this.ComposeHeader);

                page.Content().Element(this.ComposeContent);

                page.Footer().Element(this.ComposeFooter);
            });
        }

        private void ComposeHeader(IContainer container)
        {
            container
                .PaddingBottom(this.padding)
                .Text(this.model.Title)
                .FontSize(this.largeFont)
                .Bold();
        }

        private void ComposeContent(IContainer container)
        {
            container.Column(column =>
            {
                column.Spacing(this.padding);

                column.Item().Element(this.ComposeMetadata);
                column.Item().Element(this.ComposeDescription);
                column.Item().Element(this.ComposeImages);
            });
        }

        private void ComposeMetadata(IContainer container)
        {
            int spacing = 10;

            container.Border(this.borderThickness)
                .Padding(this.padding)
                .Column(col =>
            {
                col.Spacing(spacing);

                col.Item().Row(row =>
                {
                    row.Spacing(spacing);

                    row.RelativeItem().Text("Betroffenes Objekt: ");
                    row.AutoItem().Text(this.model.Object ?? "-")
                        .AlignRight();
                });

                col.Item().Row(row =>
                {
                    row.Spacing(spacing);

                    row.RelativeItem().Column(innerColumn =>
                    {
                        innerColumn.Spacing(spacing);

                        innerColumn.Item().Row(innerRow =>
                        {
                            innerRow.Spacing(spacing);

                            innerRow.RelativeItem().Text("Gebäude: ");
                            innerRow.AutoItem().Text(this.model.Building?.Name ?? "-")
                                .AlignRight();
                        });

                        innerColumn.Item().Row(innerRow =>
                        {
                            innerRow.Spacing(spacing);

                            innerRow.RelativeItem().Text("Raum: ");
                            innerRow.AutoItem().Text(this.model.Room ?? "-")
                                .AlignRight();
                        });
                    });

                    row.RelativeItem().Column(innerColumn =>
                    {
                        innerColumn.Spacing(spacing);

                        innerColumn.Item().Row(innerRow =>
                        {
                            innerRow.Spacing(spacing);

                            innerRow.RelativeItem().Text("Gemeldet durch: ");
                            innerRow.AutoItem().Text(this.model.Author)
                                .AlignRight();
                        });

                        innerColumn.Item().Row(innerRow =>
                        {
                            innerRow.Spacing(spacing);

                            innerRow.RelativeItem(10).Text("Gemeldet am: ");
                            innerRow.AutoItem().Text(this.model.CreationDate.ToShortDateString())
                                .AlignRight();
                        });
                    });
                });
            });
        }

        private void ComposeDescription(IContainer container)
        {
            container.Border(this.borderThickness)
                .Padding(this.padding)
                .Text(this.model.Description);
        }

        private void ComposeImages(IContainer container)
        {
            container.Border(this.borderThickness)
                .Padding(this.padding)
                .Row(row =>
                {
                    foreach (var image in model.Attachments)
                    {
                        row.RelativeItem().Image(model.Attachments[0].Binary);
                    }
                });
        }

        private void ComposeFooter(IContainer container)
        {
            container
                .PaddingTop(this.padding)
                .Text(DateTime.Now.ToLongDateString())
                .FontSize(this.smallFont)
                .AlignRight();
        }
    }
}
