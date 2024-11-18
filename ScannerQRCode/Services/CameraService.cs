using Emgu.CV.Structure;
using Emgu.CV;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Text.RegularExpressions;
using ZXing;
using ScannerQRCode.Entities;
using ZXing.Windows.Compatibility;
using System.Windows.Media.Media3D;
using System.IO;
using System.Windows.Media;

namespace ScannerQRCode.Services
{
    public class CameraService
    {
  
        public Bitmap ConvertToBitmap(Image<Bgr, byte> image)
        {
            // Đổi Image<Bgr, byte> thành Bitmap
            return image.ToBitmap();
        }

        public BitmapImage BitmapToBitmapImage(Bitmap bitmap)
        {
            using (var memory = new System.IO.MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                return bitmapImage;
            }
        }
        public BitmapImage ConverIdToBarcodeImage(string id, int width, int height)
        {
            var barcodeWriter = new BarcodeWriter
            {
                Format = BarcodeFormat.CODE_128,
                Options = new ZXing.Common.EncodingOptions
                {
                    Width = width,
                    Height = height
                }
            };
            Bitmap  imageBitmap = barcodeWriter.Write(id);
            return BitmapToBitmapImage(imageBitmap);

        }
        public Bitmap ConvertImageSourceToBitmap(ImageSource imageSource)
        {
            if (imageSource == null)
                return null;

            BitmapSource bitmapSource = imageSource as BitmapSource;

            if (bitmapSource != null)
            {
                // Chuyển đổi BitmapSource thành MemoryStream
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    BitmapEncoder encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
                    encoder.Save(memoryStream);

                    // Tạo Bitmap từ MemoryStream
                    using (var bmp = new Bitmap(memoryStream))
                    {
                        return new Bitmap(bmp); // Tạo một bản sao để tránh lỗi "stream is closed"
                    }
                }
            }
            return null;
        }
    }
}
