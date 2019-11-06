using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace ImgExtractor
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class HtmlImageExtractorBuilder
    {
        private readonly HtmlImageExtractor extractor;

        private HtmlImageExtractorBuilder() { }

        internal HtmlImageExtractorBuilder(IServiceCollection services)
        {
            extractor = new HtmlImageExtractor(null);
            services.AddSingleton<HtmlImageExtractor>(extractor);
        }

        internal HtmlImageExtractorBuilder(IServiceCollection services, IImageUploader imageUploader)
        {
            extractor = new HtmlImageExtractor(imageUploader);
            services.AddSingleton<HtmlImageExtractor>(extractor);
        }

        /// <summary>
        /// If <paramref name="useRandomName"/> is true, random name will be generated for file.
        /// </summary>
        public HtmlImageExtractorBuilder UseRandomName(bool useRandomName = true)
        {
            extractor.RandomName = useRandomName;
            return this;
        }

        /// <summary>
        /// Add instance of <see cref="IDownloader"/> to <see cref="HtmlImageExtractor" />
        /// </summary>
        /// <param name="downloader"></param>
        public HtmlImageExtractorBuilder AddDownloader(IDownloader downloader)
        {
            this.extractor.AddDownloader(downloader);
            return this;
        }
    }
}
