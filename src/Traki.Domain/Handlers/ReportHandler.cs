using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PuppeteerSharp;
using RazorLight;
using System.Reflection;

namespace Traki.Domain.Handlers
{
    public interface IReportHandler
    {
        Task<byte[]> GenerateHtmlReport();
        string GenerateReport();
        string SignReport();
    }

    public class ReportHandler : IReportHandler
    {
        public async Task<byte[]> GenerateHtmlReport()
        {

            string currentPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var engine = new RazorLightEngineBuilder()
                .UseFileSystemProject(currentPath + @"\Templates")
                .UseMemoryCachingProvider()
                .Build();

            var model = new { SomeText = "asdasd" };

            string result = await engine.CompileRenderAsync("Protocol.cshtml", model);

            using var browserFetcher = new BrowserFetcher();
            await browserFetcher.DownloadAsync(BrowserFetcher.DefaultChromiumRevision);
            var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = true
            });

            string fileName = Guid.NewGuid().ToString() + ".pdf";

            string filePath = currentPath + @"\" + fileName;

            using (var page = await browser.NewPageAsync())
            {
                await page.SetContentAsync(result);
                var r = await page.GetContentAsync();
                await page.PdfAsync(filePath, new PdfOptions { PrintBackground = true });
            }

            var a = File.ReadAllBytes(filePath);

            return a;
        }
        public string GenerateReport()
        {
            // Create a new PDF document
            PdfDocument document = new PdfDocument();
            document.Info.Title = "Created with PDFsharp";

            // Create an empty page
            PdfPage page = document.AddPage();

            // Get an XGraphics object for drawing
            XGraphics gfx = XGraphics.FromPdfPage(page);

            // Create a font
            XFont font = new XFont("Verdana", 20, XFontStyle.BoldItalic);

            // Draw the text
            gfx.DrawString($"Ataskaita {DateTime.Now.ToString("f")}", font, XBrushes.Black,
              new XRect(0, 0, page.Width, page.Height),
              XStringFormats.Center);


            using MemoryStream stream = new MemoryStream();

            document.Save(stream, closeStream: false);

            return Convert.ToBase64String(stream.ToArray());
        }

        public string SignReport()
        {
            throw new NotImplementedException();
        }
    }
}
