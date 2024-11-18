using Emgu.CV;
using Emgu.CV.Structure;
using ScannerQRCode.Entities;
using ScannerQRCode.Services;
using System.Windows;
using System.Windows.Threading;
using ZXing;

namespace ScannerQRCode
{
    /// <summary>
    /// Interaction logic for ScanWindow.xaml
    /// </summary>
    public partial class ScanWindow : Window
    {
        private VideoCapture _captureVideo = new VideoCapture(); // Khởi tạo camera
        private CameraService _cameraService = new();
        private bool _isQRCodeScanned = false;
        public bool IsAddProductMode { get; set; }
        private QRCodeService _qrCodeService = new QRCodeService();
        private DispatcherTimer _timer = new DispatcherTimer();
        private ProductService _productService = new ProductService();
        private string _qrContent;
        public string BarCodeResult { get; private set; }
        public ScanWindow()
        {
            InitializeComponent();
            _captureVideo.ImageGrabbed += ProcessFrame; // Đăng ký sự kiện xử lý mỗi khung hình từ camera
            _captureVideo.Start(); // Bắt đầu lấy hình ảnh từ camera
            _timer.Interval = TimeSpan.FromMilliseconds(2000); // Cài đặt thời gian chờ giữa các lần xử lý (30ms)
            _timer.Tick += (s, args) => { ProcessFrame(null, null); }; // Gọi ProcessFrame mỗi 30ms
            _timer.Start(); // Bắt đầu timer
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }
        private void ProcessFrame(object sender, EventArgs e)
        {
            if (_isQRCodeScanned) return;
            if (_captureVideo != null && _captureVideo.Ptr != IntPtr.Zero)
            {
                using (var frame = _captureVideo.QueryFrame().ToImage<Bgr, byte>())
                {

                    // Chuyển đổi hình ảnh trong luồng UI
                    Dispatcher.Invoke(() =>
                    {
                        var bitmap = _cameraService.ConvertToBitmap(frame);
                        var bitmapImage = _cameraService.BitmapToBitmapImage(bitmap);
                        imgWebcam.Source = bitmapImage; // Hiển thị hình ảnh từ camera lên điều khiển Image

                        //Xử lí mã QR
                        Result result = _qrCodeService.ProcessQRCode(bitmap);
                        if (result != null)
                        {
                            _isQRCodeScanned = true;

                            _qrContent = result.Text;
                            // Kiểm tra nếu nội dung là một liên kết URL
                            if (_qrCodeService.IsURL(_qrContent))
                            {
                                MessageBoxResult messageBoxResult = MessageBox.Show($"Liên kết URL được quét: {_qrContent}. Bạn có muốn đi tới trang này hay không", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Question);
                                if (messageBoxResult == MessageBoxResult.Yes)
                                {
                                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                                    {
                                        FileName = _qrContent,
                                        UseShellExecute = true
                                    });
                                }
                                else
                                {
                                }
                                this.Close();
                            }
                            else
                            {
                                if (IsAddProductMode)
                                {
                                    _isQRCodeScanned = true;
                                    this.BarCodeResult = result.Text; // Lưu kết quả quét vào thuộc tính BarCodeResult
                                    this.DialogResult = true; // Đóng cửa sổ với kết quả thành công
                                    this.Close();
                                    return;
                                }
                                ProductWindow productWindow = new ProductWindow();
                                Product product = _productService.GetProductById(_qrContent);
                                if (product != null)
                                {
                                    productWindow.product = product;
                                    this.Hide();
                                    productWindow.ShowDialog();
                                }
                                else
                                {
                                    
                                    productWindow = new ProductWindow();
                                    Product newProduct = new();
                                    newProduct.Id = _qrContent;
                                    productWindow.NewProduct = newProduct;
                                    this.Hide();
                                    productWindow.ShowDialog();
                                    
                                   
                                }
                            }
                        }
                    });
                }
            }
        }

        public void StopCamera()
        {
            if (_captureVideo != null && _captureVideo.IsOpened)
            {
                _captureVideo.Stop();
                _captureVideo.ImageGrabbed -= ProcessFrame; // Bỏ đăng ký sự kiện để dừng xử lý thêm khung hình 
            }

            // Đảm bảo dừng DispatcherTimer nếu chưa được dừng
            if (_timer.IsEnabled)
            {
                _timer.Stop();
            }

            _captureVideo?.Dispose(); // Dọn dẹp tài nguyên camera
        }
        protected override void OnClosed(EventArgs e)
        {
            StopCamera();
            _captureVideo?.Dispose(); // Dọn dẹp tài nguyên camera
            base.OnClosed(e);
        }
    }
}
