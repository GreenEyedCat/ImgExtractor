using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;

namespace ImgExtractor
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class DownloaderChooser : IDownloader
    {
        private readonly List<IDownloader> downloaders = new List<IDownloader>();

        public DownloaderChooser()
        {
            downloaders.Add(new HttpDownloader());
            downloaders.Add(new DataUrlDownloader());
        }

        public void AddDownloader(IDownloader downloader)
        {
            if(downloaders.LongCount(x => x.GetType() == downloader.GetType()) == 0)
            {
                downloaders.Add(downloader);
            }
            else
            {
                throw new ArgumentException($"Downloader of type {downloader.GetType().FullName} already in downloaders list.", nameof(downloader));
            }
        }

        public byte[] Download(string url)
        {
            foreach (var downloader in downloaders)
            {
                if (downloader.IsUrlSupported(url))
                {
                    return downloader.Download(url);
                }
            }
            throw new ArgumentException($"Url {url} is not supported by any of registered downloaders.", nameof(url));
        }

        public async Task<byte[]> DownloadAsync(string url)
        {
            foreach (var downloader in downloaders)
            {
                if (downloader.IsUrlSupported(url))
                {
                    return await downloader.DownloadAsync(url);
                }
            }
            throw new ArgumentException($"Url {url} is not supported by any of registered downloaders.", nameof(url));
        }

        public bool IsUrlSupported(string url)
        {
            foreach (var downloader in downloaders)
            {
                if (downloader.IsUrlSupported(url))
                {
                    return true;
                }
            }
            return false;
        }

        public string GetExtension(string url)
        {
            foreach (var downloader in downloaders)
            {
                if (downloader.IsUrlSupported(url))
                {
                    return downloader.GetExtension(url);
                }
            }
            throw new ArgumentException($"Url {url} is not supported by any of registered downloaders.", nameof(url));
        }

        public string GetFileName(string url)
        {
            foreach (var downloader in downloaders)
            {
                if (downloader.IsUrlSupported(url))
                {
                    return downloader.GetFileName(url);
                }
            }
            throw new ArgumentException($"Url {url} is not supported by any of registered downloaders.", nameof(url));
        }
    }
}
