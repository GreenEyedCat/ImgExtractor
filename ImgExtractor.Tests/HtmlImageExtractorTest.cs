using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ImgExtractor.Tests
{
    public class TestUploader : IImageUploader
    {
        private readonly Dictionary<string, byte[]> images = new Dictionary<string, byte[]>();
        private readonly List<string> urls = new List<string>();

        public byte[] GetImage(string url)
        {
            return images[url];
        }

        public byte[] GetImage(int n)
        {
            return images[urls[n]];
        }

        public string GetImageUrl(int n)
        {
            return urls[n];
        }

        public string UploadImage(string name, byte[] data)
        {
            images.Add(name, data);
            urls.Add(name);
            return name;
        }

        public Task<string> UploadImageAsync(string name, byte[] data)
        {
            throw new NotImplementedException();
        }
    }

    public class HtmlImageExtractorTest
    {
        private HtmlImageExtractor extractor;
        private TestUploader uploader;

        [SetUp]
        public void Setup()
        {
            uploader = new TestUploader();
            extractor = new HtmlImageExtractor(uploader);
        }

        [Test]
        public void ExtractImagesFromHtmlTest()
        {
            var processed = extractor.ExtractImagesFromHtml("<div><img src=\"data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAQAAAC1HAwCAAAAC0lEQVR42mP8/x8AAwMCAO+ip1sAAAAASUVORK5CYII=\"></div>").Html;
            var testFile = new byte[] {
                0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A, 0x00, 0x00, 0x00, 0x0D, 0x49, 0x48, 0x44, 0x52,
                0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x08, 0x04, 0x00, 0x00, 0x00, 0xB5, 0x1C, 0x0C,
                0x02, 0x00, 0x00, 0x00, 0x0B, 0x49, 0x44, 0x41, 0x54, 0x78, 0xDA, 0x63, 0xFC, 0xFF, 0x1F, 0x00,
                0x03, 0x03, 0x02, 0x00, 0xEF, 0xA2, 0xA7, 0x5B, 0x00, 0x00, 0x00, 0x00, 0x49, 0x45, 0x4E, 0x44,
                0xAE, 0x42, 0x60, 0x82,
            };
            var extracted = uploader.GetImage(0);
            Assert.IsTrue(processed.StartsWith("<div><img src=\""));
            Assert.IsTrue(processed.EndsWith(".png\"></div>"));
            Assert.AreEqual(testFile, extracted);
        }
    }
}
