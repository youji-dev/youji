using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using System.Globalization;

namespace DomainLayer.BusinessLogic.PDF
{
    public class TicketExportDocument(TicketExportModel ticketExportModel) : IDocument
    {
        private readonly TicketExportModel model = ticketExportModel;

        private readonly int pagePadding = 10;
        private readonly int containerPadding = 20;
        private readonly int itemSpacing = 10;

        private readonly float borderThickness = 0.5F;

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
                page.Margin(this.pagePadding);

                page.Header().Element(this.ComposeHeader);

                page.Content().Element(this.ComposeContent);

                page.Footer().Element(this.ComposeFooter);
            });
        }

        private void ComposeHeader(IContainer container)
        {
            container
                .PaddingBottom(this.containerPadding)
                .Text(this.model.Title)
                .FontSize(this.largeFont)
                .Bold();
        }

        private void ComposeContent(IContainer container)
        {
            container.Column(column =>
            {
                column.Spacing(this.containerPadding);

                column.Item().Element(this.ComposeMetadata);
                column.Item().Element(this.ComposeDescription);
                column.Item().Element(this.ComposeImages);
            });
        }

        private void ComposeMetadata(IContainer container)
        {
            container
                .Border(this.borderThickness)
                .Padding(this.containerPadding)
                .Column(col =>
            {
                col.Spacing(this.itemSpacing);

                col.Item().Row(row =>
                {
                    row.Spacing(this.itemSpacing);

                    row.RelativeItem().Text("Betroffenes Objekt: ");
                    row.AutoItem().Text(this.model.Object ?? "-")
                        .AlignRight();
                });

                col.Item().Row(row =>
                {
                    row.Spacing(this.itemSpacing);

                    row.RelativeItem().Column(innerColumn =>
                    {
                        innerColumn.Spacing(this.itemSpacing);

                        innerColumn.Item().Row(innerRow =>
                        {
                            innerRow.Spacing(this.itemSpacing);

                            innerRow.RelativeItem().Text("Gebäude: ");
                            innerRow.AutoItem().Text(this.model.Building?.Name ?? "-")
                                .AlignRight();
                        });

                        innerColumn.Item().Row(innerRow =>
                        {
                            innerRow.Spacing(this.itemSpacing);

                            innerRow.RelativeItem().Text("Raum: ");
                            innerRow.AutoItem().Text(this.model.Room ?? "-")
                                .AlignRight();
                        });
                    });

                    row.RelativeItem().Column(innerColumn =>
                    {
                        innerColumn.Spacing(this.itemSpacing);

                        innerColumn.Item().Row(innerRow =>
                        {
                            innerRow.Spacing(this.itemSpacing);

                            innerRow.RelativeItem().Text("Gemeldet durch: ");
                            innerRow.AutoItem().Text(this.model.Author)
                                .AlignRight();
                        });

                        innerColumn.Item().Row(innerRow =>
                        {
                            innerRow.Spacing(this.itemSpacing);

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
            container
                .Padding(this.containerPadding)
                .Text(this.model.Description);
        }

        private void ComposeImages(IContainer container)
        {
            if (this.model.Images is null || this.model.Images.Length == 0)
                return;

            container
                .Border(this.borderThickness)
                .Padding(this.containerPadding)
                .Inlined(inlined =>
                {
                    inlined.Spacing(this.itemSpacing);

                    for (int i = 0; i < this.model.Images.Length; i++)
                    {
                        inlined.Item()
                            .Column(col =>
                            {
                                col.Item().Height(100).Image(this.model.Images[i].data);
                                col.Item().Text(this.model.Images[i].name)
                                    .FontSize(this.smallFont)
                                    .AlignCenter();
                            });
                    }
                });
        }

        private void ComposeFooter(IContainer container)
        {
            container
                .PaddingTop(this.containerPadding)
                .Text(DateTime.Now.ToLongDateString())
                .FontSize(this.smallFont)
                .AlignRight();
        }
    }
}
