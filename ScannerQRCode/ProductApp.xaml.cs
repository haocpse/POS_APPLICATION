using ScannerQRCode.Entities;
using ScannerQRCode.Services;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using ZXing;
using ZXing.Windows.Compatibility;

namespace ScannerQRCode
{
    /// <summary>
    /// Interaction logic for ProductApp.xaml
    /// </summary>
    public partial class ProductApp : Window
    {
        ProductService productService = new();
        public ProductApp()
        {
            InitializeComponent();
          
        }

        private void btnGenerateBarcode_Click(object sender, RoutedEventArgs e)
        {
            Product product = new()
            {
                Id = GenerateRandomId(),
                ProductName = txtProductName.Text,
                Price = Double.Parse(txtProductPrice.Text),
                Quantity = int.Parse(txtProductQuantity.Text),
            };
            var barcodeWriter = new BarcodeWriter
            {
                Format = BarcodeFormat.CODE_128,
                Options = new ZXing.Common.EncodingOptions
                {
                    Width = 200,
                    Height = 100
                }
            };
            var barcodeImage = barcodeWriter.Write(product.Id);
            imgBarcode.Source = BitmapToImageSource(barcodeImage);
            var barcodePath = @$"Z:\Pictures\{product.ProductName}.png";
            productService.AddProduct(product);
            barcodeImage.Save(barcodePath);
        }

        private string GenerateRandomId()
        {
            Random random = new Random();
            return random.Next(100000000, 999999999).ToString() + random.Next(100, 999).ToString();
        }

        private BitmapImage BitmapToImageSource(Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                return bitmapImage;
            }
        }
    }
}
