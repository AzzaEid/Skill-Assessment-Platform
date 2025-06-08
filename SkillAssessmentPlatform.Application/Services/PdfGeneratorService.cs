using System;
using DinkToPdf;
using DinkToPdf.Contracts;


namespace SkillAssessmentPlatform.Application.Services
{
    public class PdfGeneratorService
    {
        private readonly IConverter _converter;

        public PdfGeneratorService(IConverter converter)
        {
            _converter = converter;
        }

        public byte[] GeneratePdfFromHtml(string htmlContent)
        {
            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings = new GlobalSettings
                {
                    PaperSize = PaperKind.A4,
                    Orientation = Orientation.Portrait
                },
                Objects = {
                    new ObjectSettings
                    {
                        HtmlContent = htmlContent,
                        WebSettings = { DefaultEncoding = "utf-8" }
                    }
                }
            };

            return _converter.Convert(doc);
        }
    }
}