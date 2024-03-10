using PuppeteerSharp;
using RazorLight;
using System.Reflection;

namespace Traki.Domain.Handlers
{
    public interface IReportGenerator
    {
        Task<string> GenerateHtmlReport(object model, string templateName);
        Task<MemoryStream> GeneratePDFReportFromHtml(string htmlContent, bool useColors);
    }
    public class ReportGenerator: IReportGenerator
    {
        private readonly RazorLightEngine _razorLightEngine;
        private readonly string _currentPath;

        public ReportGenerator()
        {
            _currentPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Directory.CreateDirectory(_currentPath + @"\Templates");
            _razorLightEngine = new RazorLightEngineBuilder()
                .UseFileSystemProject(_currentPath + @"\Templates")
                .UseMemoryCachingProvider()
                .Build();
        }

        public async Task<string> GenerateHtmlReport(object model, string templateName)
        {
            return await _razorLightEngine.CompileRenderAsync(templateName, model);
        }

        public async Task<MemoryStream> GeneratePDFReportFromHtml(string htmlContent, bool useColors)
        {
            using var browserFetcher = new BrowserFetcher();
            await browserFetcher.DownloadAsync();
            var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = true
            });

            string fileName = Guid.NewGuid().ToString() + ".pdf";

            string filePath = _currentPath + @"\" + fileName;

            using (var page = await browser.NewPageAsync())
            {
                await page.SetContentAsync(htmlContent);
                await page.PdfAsync(filePath, new PdfOptions { PrintBackground = useColors });
            }

            var pdfAsBytes = File.ReadAllBytes(filePath);
            File.Delete(filePath);
            return new MemoryStream(pdfAsBytes);
        }
    }
}
