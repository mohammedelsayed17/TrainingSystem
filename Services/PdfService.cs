using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.IO;

namespace TrainingSystem.Services
{
    public class PdfService
    {
         public byte[] GenerateCertificate(string traineeName, string courseName, double degree, byte[] qrCodeImage)
        {
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(30);
                    page.Size(PageSizes.A4);

                    page.Content().Column(col =>
                    {
                        col.Item().Text("Training Certificate").FontSize(24).Bold().AlignCenter();
                        col.Item().Text($"Trainee: {traineeName}").FontSize(18);
                        col.Item().Text($"Course: {courseName}").FontSize(18);
                        col.Item().Text($"Degree: {degree}/1000").FontSize(18);
                        col.Item().Image(qrCodeImage).FitWidth();
                    });
                });
            });

            return document.GeneratePdf();
        }

        internal byte[] GenerateCoursePdf(string name, double degree, string deptName, byte[] qr)
        {
            throw new NotImplementedException();
        }
    }
}