using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ImgExtractor
{
    /// <summary>
    /// Uploads image to storage.
    /// </summary>
    public interface IImageUploader
    {
        /// <summary>
        /// Uploads image.
        /// </summary>
        /// <param name="name">New filename for file.</param>
        /// <param name="data">Content of file.</param>
        /// <returns>Url of saved file</returns>
        string UploadImage(string name, byte[] data);

        /// <summary>
        /// Uploads image asynchronously.
        /// </summary>
        /// <param name="name">New filename for file.</param>
        /// <param name="data">Content of file.</param>
        /// <returns>Url of saved file</returns>
        Task<string> UploadImageAsync(string name, byte[] data);
    }
}
