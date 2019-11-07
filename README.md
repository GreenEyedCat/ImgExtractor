[![NuGet][main-nuget-badge]][main-nuget] [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

[main-nuget]: https://www.nuget.org/packages/ImgExtractor/
[main-nuget-badge]: https://img.shields.io/nuget/v/ImgExtractor.svg 

## Summary
ImgExtractor is a library that extracts images from html (http links as well as data URIs), save in place you want and replaces <img> sources in html.

##  Usage

Initialization in startup.cs:
```csharp
public void ConfigureServices(IServiceCollection services)
{
    //...
    services.AddSingleton<LocalImageSaver>();          //Register implementation of IImageUploader in IServiceCollection
    services.AddHtmlImageExtractor<LocalImageSaver>()  //Add HtmlImageExtractor to IServiceCollection
        .UseRandomName();                              //You should call this if you want to generate random names for files.
    //...
}
```

Example of implementaion of IImageUploader:
```csharp
//your implementation of IImageUploader, which saves files in place you want
public class LocalImageSaver : IImageUploader
{
    private readonly IHostEnvironment hostEnvironment;

    public LocalImageSaver(IHostEnvironment hostEnvironment)
    {
        this.hostEnvironment = hostEnvironment;
    }

    //name is a file name
    //data[] is a file content
    //function should return new url of file
    public string UploadImage(string name, byte[] data)
    {
        string filePath = Path.Combine(hostEnvironment.ContentRootPath, "wwwroot", "files", name);
        File.WriteAllBytes(filePath, data);
        var url = "/files/" + name;
        return url;
    }

    public async Task<string> UploadImageAsync(string name, byte[] data)
    {
        string filePath = Path.Combine(hostEnvironment.ContentRootPath, "wwwroot", "files", name);
        await File.WriteAllBytesAsync(filePath, data);
        var url = "/files/" + name;
        return url;
    }
}
```
Usage in controller:
```csharp
var extractionResult = imageExtractor.ExtractImagesFromHtml(html); //parse html and replace <img> sources with links to reuploaded files
//You can use ExtractImagesFromHtmlAsync() in async methods
SomeVeryUsefulFunction(extractionResult.Mapping);                  //extractionResult.Mapping is a dictionary that contains original image urls as keys and new image urls as values
SaveHtml(extractionResult.Html);                                   //extractionResult.Html contains html with replaced <img> sources
```