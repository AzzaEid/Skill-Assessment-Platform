using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Hosting;

namespace SkillAssessmentPlatform.Application.Services
{
    public class PdfGeneratorService
    {
        private readonly IConverter _converter;
        private readonly IWebHostEnvironment _environment;

        public PdfGeneratorService(IConverter converter, IWebHostEnvironment environment)
        {
            _converter = converter;
            _environment = environment;
        }

        public byte[] GeneratePdfFromHtml(string htmlContent)
        {
            var processedHtml = ProcessHtmlForPdf(htmlContent);

            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings = new GlobalSettings
                {
                    PaperSize = PaperKind.A4,
                    Orientation = Orientation.Portrait,
                    Margins = new MarginSettings
                    {
                        Left = 10,
                        Right = 10,
                        Top = 10,
                        Bottom = 10
                    },
                    DPI = 300,
                    ColorMode = ColorMode.Color
                },
                Objects = {
                    new ObjectSettings
                    {
                        HtmlContent = processedHtml,
                        WebSettings = new WebSettings
                        {
                            DefaultEncoding = "utf-8",
                            LoadImages = true,
                            PrintMediaType = true,
                            EnableJavascript = false
                        }
                    }
                }
            };

            return _converter.Convert(doc);
        }

        private string ProcessHtmlForPdf(string htmlContent)
        {
            // إزالة العناصر التي قد تتداخل مع PDF
            htmlContent = htmlContent.Replace("onclick=\"window.print()\"", "");
            htmlContent = RemoveActionButtons(htmlContent);

            return htmlContent;
        }

        private string RemoveActionButtons(string html)
        {
            // إزالة أزرار التحميل والطباعة من PDF
            var startIndex = html.IndexOf("<div class=\"action-buttons\">");
            if (startIndex >= 0)
            {
                var endIndex = html.IndexOf("</div>", startIndex) + 6;
                if (endIndex > startIndex)
                {
                    html = html.Remove(startIndex, endIndex - startIndex);
                }
            }
            return html;
        }


    }
}