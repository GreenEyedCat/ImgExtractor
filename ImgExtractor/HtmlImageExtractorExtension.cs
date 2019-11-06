using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ImgExtractor
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class HtmlImageExtractorExtension
    {
        /// <summary>
        /// Register <see cref="HtmlImageExtractor"/> without <see cref="IImageUploader"/> in <see cref="IServiceCollection"/>.
        /// </summary>
        public static HtmlImageExtractorBuilder AddHtmlImageExtractor(this IServiceCollection services)
        {
            return new HtmlImageExtractorBuilder(services);
        }

        /// <summary>
        /// Register <see cref="HtmlImageExtractor"/>, which uses particular <see cref="IImageUploader"/> in <see cref="IServiceCollection"/>
        /// </summary>
        /// <param name="services"></param>
        /// <param name="imageUploader">Implementation of <see cref="IImageUploader"/> to save extracted files.</param>
        public static HtmlImageExtractorBuilder AddHtmlImageExtractor(this IServiceCollection services, IImageUploader imageUploader)
        {
            return new HtmlImageExtractorBuilder(services, imageUploader);
        }

        /// <summary>
        /// Register <see cref="HtmlImageExtractor"/>, which uses <see cref="IImageUploader"/> from <see cref="IServiceCollection"/>
        /// </summary>
        /// <typeparamref name="T">Type of <see cref="IImageUploader"/></typeparamref>
        public static HtmlImageExtractorBuilder AddHtmlImageExtractor<T>(this IServiceCollection services)
        {
            var provider = services.BuildServiceProvider(false);
            var imageUploader = provider.GetRequiredService<T>() as IImageUploader;
            return new HtmlImageExtractorBuilder(services, imageUploader);
        }
    }
}
