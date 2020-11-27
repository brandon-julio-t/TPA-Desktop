using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;
using QRCoder;
using TPA_Desktop.Core.Models;

namespace TPA_Desktop.Core.Decorators
{
    public class QrCodeDecorator
    {
        private static readonly QRCodeGenerator Generator = new QRCodeGenerator();

        private readonly QRCode _qrCode;

        public QrCodeDecorator(QrCode qrCode)
        {
            var payload = new PayloadGenerator.Url(qrCode.Url).ToString();
            var data = Generator.CreateQrCode(payload, QRCodeGenerator.ECCLevel.Q);
            _qrCode = new QRCode(data);
        }

        public BitmapImage ToImage()
        {
            using (var memory = new MemoryStream())
            {
                var bitmap = _qrCode.GetGraphic(20);

                bitmap.Save(memory, ImageFormat.Bmp);
                memory.Position = 0;

                var image = new BitmapImage();
                image.BeginInit();
                image.StreamSource = memory;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.EndInit();

                return image;
            }
        }
    }
}