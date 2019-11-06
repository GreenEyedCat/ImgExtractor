using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImgExtractor.Tests
{
    public class TestDownloader : IDownloader
    {
        private const string testUrl = "test:";

        public byte[] Download(string url)
        {
            if(url.StartsWith(testUrl))
            {
                return Encoding.ASCII.GetBytes(url.Substring(testUrl.Length));
            }
            return null;
        }

        public Task<byte[]> DownloadAsync(string url)
        {
            throw new NotImplementedException();
        }

        public string GetExtension(string url)
        {
            throw new NotImplementedException();
        }

        public string GetFileName(string url)
        {
            throw new NotImplementedException();
        }

        public bool IsUrlSupported(string url)
        {
            return url.StartsWith(testUrl);
        }
    }

    public class TestHexDownloader : IDownloader
    {
        private const string testUrl = "hex:";

        private static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }
        public byte[] Download(string url)
        {
            if (url.StartsWith(testUrl))
            {
                var hex = url.Substring(testUrl.Length);
                return StringToByteArray(hex);
            }
            return null;
        }

        public Task<byte[]> DownloadAsync(string url)
        {
            throw new NotImplementedException();
        }

        public string GetExtension(string url)
        {
            throw new NotImplementedException();
        }

        public string GetFileName(string url)
        {
            throw new NotImplementedException();
        }

        public bool IsUrlSupported(string url)
        {
            return url.StartsWith(testUrl);
        }
    }

    public class DownloaderChooserTest
    {
        private DownloaderChooser downloaderChooser;

        [SetUp]
        public void Setup()
        {
            downloaderChooser = new DownloaderChooser();
            downloaderChooser.AddDownloader(new TestDownloader());
            downloaderChooser.AddDownloader(new TestHexDownloader());
        }

        [TestCase("test:hello", true)]
        [TestCase("hex:616263", true)]
        [TestCase("abc://def", false)]
        public void IsUrlSupportedTest(string url, bool supported)
        {
            var isSupported = downloaderChooser.IsUrlSupported(url);
            Assert.AreEqual(supported, isSupported);
        }

        [TestCase("test:hello", new byte[] { 0x68, 0x65, 0x6C, 0x6C, 0x6F })]
        [TestCase("hex:616263", new byte[] { 0x61, 0x62, 0x63 })]
        public void ExtractTest(string url, byte[] excepted)
        {
            var result = downloaderChooser.Download(url);
            Assert.AreEqual(excepted, result);
        }
    }
}
