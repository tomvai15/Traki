using Microsoft.AspNetCore.Mvc;
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;

namespace Traki.Api.Controllers
{
    [Route("api/reports")]
    public class ReportsController: ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<string>> GetTemplates()
        {
            // TODO: Move to seperate component
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
    }
}
