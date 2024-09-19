using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace DomainLayer.BusinessLogic.PDF
{
    /// <summary>
    /// QuestPDF document template for ticket export
    /// </summary>
    /// <param name="ticketExportModel">The model to use</param>
    public class TicketExportDocument(TicketExportModel ticketExportModel) : IDocument
    {
        private readonly TicketExportModel model = ticketExportModel;

        private readonly int pagePadding = 10;
        private readonly int horizontalPadding = 20;
        private readonly int verticalPadding = 10;
        private readonly int itemSpacing = 10;

        private readonly float borderThickness = 0.5F;

        private readonly int smallFont = 10;
        private readonly int largeFont = 20;

        /// <inheritdoc/>
        public DocumentMetadata GetMetadata() => new()
        {
            Author = "youji export",
            Creator = "youji export",
            Producer = "youji export",
            Title = this.model.Title,
            Subject = this.model.Title,
        };

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
                .PaddingHorizontal(this.horizontalPadding)
                .PaddingBottom(this.verticalPadding)
                .Text(this.model.Title)
                .FontSize(this.largeFont)
                .Bold();
        }

        private void ComposeContent(IContainer container)
        {
            container.Column(column =>
            {
                column.Spacing(this.itemSpacing);

                column.Item().Element(this.ComposeMetadata);
                column.Item().Element(this.ComposeDescription);
                column.Item().Element(this.ComposeImages);
            });
        }

        private void ComposeMetadata(IContainer container)
        {
            container
                .Border(this.borderThickness)
                .PaddingHorizontal(this.horizontalPadding)
                .PaddingVertical(this.verticalPadding)
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

                col.Item().Row(innerRow =>
                {
                    innerRow.Spacing(this.itemSpacing);

                    innerRow.RelativeItem().Text("Gebäude: ");
                    innerRow.AutoItem().Text(this.model.Building ?? "-")
                        .AlignRight();
                });

                col.Item().Row(innerRow =>
                {
                    innerRow.Spacing(this.itemSpacing);

                    innerRow.RelativeItem().Text("Raum: ");
                    innerRow.AutoItem().Text(this.model.Room ?? "-")
                        .AlignRight();
                });

                col.Item().Row(innerRow =>
                {
                    innerRow.Spacing(this.itemSpacing);

                    innerRow.RelativeItem().Text("Gemeldet durch: ");
                    innerRow.AutoItem().Text(this.model.Author)
                        .AlignRight();
                });

                col.Item().Row(innerRow =>
                {
                    innerRow.Spacing(this.itemSpacing);

                    innerRow.RelativeItem(10).Text("Gemeldet am: ");
                    innerRow.AutoItem().Text(this.model.CreationDate.ToShortDateString())
                        .AlignRight();
                });
            });
        }

        private void ComposeDescription(IContainer container)
        {
            container
                .PaddingHorizontal(this.horizontalPadding)
                .PaddingVertical(this.verticalPadding)
                .Text(this.model.Description);
        }

        private void ComposeImages(IContainer container)
        {
            if (this.model.Images is null || this.model.Images.Length == 0)
                return;

            container
                .Border(this.borderThickness)
                .PaddingHorizontal(this.horizontalPadding)
                .PaddingVertical(this.verticalPadding)
                .Inlined(inlined =>
                {
                    inlined.Spacing(this.itemSpacing);

                    for (int i = 0; i < this.model.Images.Length; i++)
                    {
                        inlined.Item()
                            .Column(col =>
                            {
                                col.Item().Height(100).Image(this.model.Images[i].Data);
                                col.Item().Text(this.model.Images[i].FileName)
                                    .FontSize(this.smallFont)
                                    .AlignCenter();
                            });
                    }
                });
        }

        private void ComposeFooter(IContainer container)
        {
            container
                .PaddingHorizontal(this.horizontalPadding)
                .PaddingTop(this.verticalPadding)
                .Text(DateTime.Now.ToLongDateString())
                .FontSize(this.smallFont)
                .AlignRight();
        }
    }
}
