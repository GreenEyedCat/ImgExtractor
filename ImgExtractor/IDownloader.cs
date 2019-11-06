using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ImgExtractor
{
    public interface IDownloader
    {
        bool IsUrlSupported(string url);
        byte[] Download(string url);
        Task<byte[]> DownloadAsync(string url);
        string GetExtension(string url);
        string GetFileName(string url);
    }
}
