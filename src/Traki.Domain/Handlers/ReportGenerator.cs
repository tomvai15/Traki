﻿using PuppeteerSharp;
using RazorLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Traki.Domain.Models;

namespace Traki.Domain.Handlers
{
    public interface IReportGenerator
    {
        Task<string> GenerateHtmlReport(object model, string templateName);
        Task<MemoryStream> GeneratePDFReportFromHtml(string htmlContent);
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

        public async Task<MemoryStream> GeneratePDFReportFromHtml(string htmlContent)
        {
            using var browserFetcher = new BrowserFetcher();
            await browserFetcher.DownloadAsync(BrowserFetcher.DefaultChromiumRevision);
            var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = true
            });

            string fileName = Guid.NewGuid().ToString() + ".pdf";

            string filePath = _currentPath + @"\" + fileName;

            using (var page = await browser.NewPageAsync())
            {
                await page.SetContentAsync(htmlContent);
                await page.PdfAsync(filePath, new PdfOptions { PrintBackground = true });
            }

            var pdfAsBytes = File.ReadAllBytes(filePath);
            File.Delete(filePath);
            return new MemoryStream(pdfAsBytes);
        }
    }
}
