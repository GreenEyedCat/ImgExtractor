using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ImgExtractor
{
    /// <summary>
    /// Representation of html with reuploaded images.
    /// </summary>
    public class ImageExtractionResult
    {
        private ImageExtractionResult() { }

        internal ImageExtractionResult(string html, Dictionary<string, string> mapping)
        {
            Html = html;
            Mapping = new ReadOnlyDictionary<string, string>(mapping);
        }

        /// <summary>
        /// Html, in which image sources was replaced to reuploaded images urls.
        /// </summary>
        public string Html { get; private set; }
        /// <summary>
        /// Mapping of old image sources to new sources. Keys is links to origin of images, values is links to reuploaded images.
        /// </summary>
        public ReadOnlyDictionary<string, string> Mapping { get; private set; }
    }
}
