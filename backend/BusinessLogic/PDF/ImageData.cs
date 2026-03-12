namespace DomainLayer.BusinessLogic.PDF
{
    /// <summary>
    /// Struct for simple image data
    /// </summary>
    public struct ImageData()
    {
        /// <summary>
        /// The images filename
        /// </summary>
        public required string FileName { get; init; }

        /// <summary>
        /// The image data in binary
        /// </summary>
        public required byte[] Data { get; init; }
    }
}
