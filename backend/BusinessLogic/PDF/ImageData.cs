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
        public string FileName { get; set; }

        /// <summary>
        /// The image data in binary
        /// </summary>
        public byte[] Data { get; set; }
    }
}
