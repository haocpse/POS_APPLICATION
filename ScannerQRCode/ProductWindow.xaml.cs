using Microsoft.Win32;
using ScannerQRCode.Entities;
using ScannerQRCode.Services;
using System.Drawing.Imaging;
using System.Drawing;
using System.Windows;
using ZXing;
using ZXing.Windows.Compatibility;



namespace ScannerQRCode
{
    /// <summary>
    /// Interaction logic for ProductWindow.xaml
    /// </summary>
    public partial class ProductWindow : Window
    {
        public Product product { get; set; }
        public Product NewProduct { get; set; }
        private ProductService _productService = new ProductService();
        private CameraService _cameraService = new CameraService();
        public ProductWindow()
        {
            InitializeComponent();
        }

        private void LoadData()
        {
            if(NewProduct != null)
            {
                IdTextBox.Text = NewProduct.Id;
            }
            if (product != null)
            {
                IdTextBox.Text = product.Id;
                NameTextBox.Text = product.ProductName;
                PriceTextBox.Text = product.Price.ToString();
                QuantityTextBox.Text = product.Quantity.ToString();
                int barcodeWidth = (int)BarcodeImage.Width;
                int barcodeHeight = (int)BarcodeImage.Height;
                BarcodeImage.Source = _cameraService.ConverIdToBarcodeImage(product.Id, barcodeWidth, barcodeHeight);
            }

            else
            {
                IdTextBox.IsReadOnly = false;
                DownloadButton.Content = "Scan Barcode";
 
            }
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            Product tempProdut = new Product
            {
                Id = IdTextBox.Text,
                ProductName = NameTextBox.Text,
                Price = double.Parse(PriceTextBox.Text),
                Quantity = int.Parse(QuantityTextBox.Text)

            };

            if (product != null)
            {
                _productService.UpdateProduct(tempProdut);
            }
            else
            {
               
                _productService.AddProduct(tempProdut);
            }
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        public void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
        }


        private void DownloadButton_Click(object sender, RoutedEventArgs e)
        {
            if (product == null)
            {
                ScanWindow scanWindow = new ScanWindow();
                scanWindow.IsAddProductMode = true;
                if (scanWindow.ShowDialog() == true) // Kiểm tra nếu quá trình quét thành công
                {
                    string scannedCode = scanWindow.BarCodeResult; // Nhận chuỗi kết quả từ ScanWindow
                    IdTextBox.Text = scannedCode;                 
                }
                return;
            }
            // Tạo dialog để chọn đường dẫn lưu ảnh
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "PNG Image|*.png",
                Title = "Save Barcode Image",
                FileName = $"{product.ProductName}_barcode.png"
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                // Chuyển đổi ImageSource của BarcodeImage thành Bitmap bằng hàm có sẵn
                Bitmap bitmap = _cameraService.ConvertImageSourceToBitmap(BarcodeImage.Source);

                // Kiểm tra nếu chuyển đổi thành công và lưu ảnh
                if (bitmap != null)
                {
                    bitmap.Save(saveFileDialog.FileName, System.Drawing.Imaging.ImageFormat.Png);
                    MessageBox.Show("Barcode image saved successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    bitmap.Dispose(); // Giải phóng tài nguyên sau khi lưu
                }
                else
                {
                    MessageBox.Show("Failed to save the barcode image.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

        }
    }
}
