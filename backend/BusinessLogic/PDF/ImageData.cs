namespace DomainLayer.BusinessLogic.PDF
{
    public record ImageData(
        string fileName,
        byte[] data)
    {
        /// <summary>
        /// The images filename
        /// </summary>
        public string FileName { get; set; } = fileName;

        /// <summary>
        /// The image data in binary
        /// </summary>
        public byte[] Data { get; set; } = data;
    }
}
