using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.IO;


namespace TrainingSystem.Services
{
    public class PdfCourseService
    {
        public byte[] GenerateCoursePdf(string courseName, double degree, string departmentName, byte[] qrCodeImage)
        {
            var document = Document.Create(container =>
     {
         container.Page(page =>
         {
             page.Margin(30);
             page.Size(PageSizes.A4);
             page.DefaultTextStyle(x => x.FontSize(16));

             page.Content()
                 .Column(col =>
                 {
                     col.Item().Text("Course info ").Bold().FontSize(24);
                     col.Item().Text($"Course: {courseName}");
                     col.Item().Text($"Max Degree: {degree}");
                     col.Item().Text($"Department: {departmentName}");
                     col.Item().PaddingVertical(15).Image(qrCodeImage, ImageScaling.FitWidth);
                 });
         });
     });

            return document.GeneratePdf();
        }
    }
}