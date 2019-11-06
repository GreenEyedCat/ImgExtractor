using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ImgExtractor
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class HttpDownloader : IDownloader
    {
        private WebClient client;

        private WebClient Client
        {
            get
            {
                if(client == null)
                {
                    client = new WebClient();
                }
                return client;
            }
        }

        public byte[] Download(string url)
        {
            var data = Client.DownloadData(url);
            return data;
        }

        public async Task<byte[]> DownloadAsync(string url)
        {
            var data = await Client.DownloadDataTaskAsync(url);
            return data;
        }

        public string GetExtension(string url)
        {
            string path;
            var index = url.IndexOf('?');
            if (index >= 0)
            {
                path = url.Substring(0, index);
            }
            else
            {
                path = url;
            }
            return Path.GetExtension(path);
        }

        public string GetFileName(string url)
        {
            string path;
            var index = url.IndexOf('?');
            if (index >= 0)
            {
                path = url.Substring(0, index);
            }
            else
            {
                path = url;
            }
            return Path.GetFileNameWithoutExtension(path);
        }

        public bool IsUrlSupported(string url)
        {
            return url.StartsWith("http://") || url.StartsWith("https://") || url.StartsWith("ftp://");
        }
    }
}
