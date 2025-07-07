using QRCoder;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
// using QRCoder.Mvc;
namespace TrainingSystem.Helpers
{
    public static class QRCodeHelper
    {
        public static byte[] GenerateQRCode(string text)
        {
             using var qrGenerator = new QRCodeGenerator();
            using var qrCodeData = qrGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);
            using var qrCode = new QRCode(qrCodeData);
            using var bitmap = qrCode.GetGraphic(5);
            using var ms = new MemoryStream();
            bitmap.Save(ms, ImageFormat.Png);
            return ms.ToArray();
        }
    }
}
