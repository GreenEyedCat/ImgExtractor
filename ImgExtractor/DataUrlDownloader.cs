using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ImgExtractor
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class DataUrlDownloader : IDownloader
    {
        public byte[] Download(string url)
        {
            var regex = Regex.Match(url, @"data:image/(?<type>.+?);base64,(?<data>.+)");
            var base64Data = regex.Groups["data"].Value;
            return Convert.FromBase64String(base64Data);
        }

        public async Task<byte[]> DownloadAsync(string url)
        {
            return await Task.Run(() => Download(url));
        }

        public string GetExtension(string url)
        {
            var regex = Regex.Match(url, @"data:image/(?<type>.+?);base64,(?<data>.+)");
            var format = regex.Groups["type"].Value;
            return "." + format;
        }

        public string GetFileName(string url)
        {
            return null;
        }

        public bool IsUrlSupported(string url)
        {
            return url.StartsWith("data:");
        }
    }
}
