using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ImgExtractor
{
    /// <summary>
    /// Class for extraction images from html.
    /// </summary>
    public class HtmlImageExtractor
    {
        private readonly IImageUploader imageUploader;
        private readonly DownloaderChooser downloaderChooser = new DownloaderChooser();

        /// <summary>
        /// If <see langword="true"/> file name will be generated randomly.
        /// </summary>
        public bool RandomName { get; set; }

        /// <summary>
        /// Add instance of <see cref="IDownloader"/> to download images.
        /// </summary>
        /// <param name="downloader"></param>
        public void AddDownloader(IDownloader downloader)
        {
            downloaderChooser.AddDownloader(downloader);
        }

        /// <summary>
        /// Class for extraction images from html.
        /// </summary>
        /// <param name="imageUploader">Implementation of <see cref="IImageUploader" /> to save extracted files.</param>
        public HtmlImageExtractor(IImageUploader imageUploader)
        {
            this.imageUploader = imageUploader;
        }

        /// <summary>
        /// Extracts images from html asynchronously.
        /// </summary>
        /// <param name="html">Html code to extract images from it.</param>
        /// <returns>Html with replaced images sources.</returns>
        public async Task<ImageExtractionResult> ExtractImagesFromHtmlAsync(string html)
        {
            Dictionary<string, string> mapping = new Dictionary<string, string>();
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);
            var imgs = htmlDocument.DocumentNode.Descendants("img");
            foreach(var img in imgs)
            {
                var src = img.GetAttributeValue("src", "");
                var newSrc = await ExtractImageAsync(src);
                img.SetAttributeValue("src", newSrc);
                mapping[src] = newSrc;
            }
            return new ImageExtractionResult(htmlDocument.DocumentNode.OuterHtml, mapping);
        }

        /// <summary>
        /// Extracts images from html.
        /// </summary>
        /// <param name="html">Html code to extract images from it.</param>
        /// <returns>Html with replaced images sources.</returns>
        public ImageExtractionResult ExtractImagesFromHtml(string html)
        {
            Dictionary<string, string> mapping = new Dictionary<string, string>();
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);
            var imgs = htmlDocument.DocumentNode.Descendants("img");
            foreach (var img in imgs)
            {
                var src = img.GetAttributeValue("src", "");
                var newSrc = ExtractImage(src);
                img.SetAttributeValue("src", newSrc);
                mapping[src] = newSrc;
            }
            return new ImageExtractionResult(htmlDocument.DocumentNode.OuterHtml, mapping);
        }

        private async Task<string> ExtractImageAsync(string src)
        {
            if (downloaderChooser.IsUrlSupported(src))
            {
                var data = await downloaderChooser.DownloadAsync(src);
                return await imageUploader.UploadImageAsync(GetFileName(src, RandomName), data);
            }
            if (src.StartsWith("/"))
            {
                return src;
            }
            return "";
        }

        private string ExtractImage(string src)
        {
            if(downloaderChooser.IsUrlSupported(src))
            {
                var data = downloaderChooser.Download(src);
                return imageUploader.UploadImage(GetFileName(src, RandomName), data);
            }
            if (src.StartsWith("/"))
            {
                return src;
            }
            return "";
        }

        private string GetFileName(string path, bool randomName)
        {
            var fileName = downloaderChooser.GetFileName(path);
            if(string.IsNullOrEmpty(fileName) || randomName)
            {
                fileName = (new Random()).Next().ToString() + (new Random()).Next().ToString();
            }
            return fileName + downloaderChooser.GetExtension(path);
        }
    }
}
