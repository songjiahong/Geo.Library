using System.IO;

namespace GeoLibrary.Extension
{
    internal static class ReaderExtension
    {
        /// <summary>
        /// Is the end of the text reader
        /// </summary>
        /// <param name="reader">target reader</param>
        /// <returns>true if reach to the end of the text, false otherwise</returns>
        public static bool IsEOT(this TextReader reader)
        {
            return reader.Peek() == -1;
        }

        public static bool IsEOT(this BinaryReader reader)
        {
            return reader.PeekChar() == -1;
        }
    }
}
