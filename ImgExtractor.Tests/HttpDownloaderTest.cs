﻿using HttpMock.Net;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImgExtractor.Tests
{
    public class HttpDownloaderTest
    {
        private HttpDownloader downloader;
        private byte[] testFile;

        [OneTimeSetUp]
        public void Setup()
        {
            downloader = new HttpDownloader();
            testFile = new byte[] {
                0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A, 0x00, 0x00, 0x00, 0x0D, 0x49, 0x48, 0x44, 0x52,
                0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x08, 0x04, 0x00, 0x00, 0x00, 0xB5, 0x1C, 0x0C,
                0x02, 0x00, 0x00, 0x00, 0x0B, 0x49, 0x44, 0x41, 0x54, 0x78, 0xDA, 0x63, 0xFC, 0xFF, 0x1F, 0x00,
                0x03, 0x03, 0x02, 0x00, 0xEF, 0xA2, 0xA7, 0x5B, 0x00, 0x00, 0x00, 0x00, 0x49, 0x45, 0x4E, 0x44,
                0xAE, 0x42, 0x60, 0x82,
            };
            var server = Server.Start(9191);
            server.WhenGet("/1.png").Do(x => x.Response.Body.Write(testFile, 0, testFile.Length));
        }

        [TestCase(@"data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAQAAAC1HAwCAAAAC0lEQVR42mP8/x8AAwMCAO+ip1sAAAAASUVORK5CYII=", false)]
        [TestCase(@"http://localhost:9191/1.png", true)]
        public void IsUrlSupportedTest(string url, bool supported)
        {
            Assert.AreEqual(supported, downloader.IsUrlSupported(url));
        }

        [Test]
        public void ExtractTest()
        {
            var data = downloader.Download("http://localhost:9191/1.png");
            Assert.AreEqual(testFile, data);
        }
    }
}
