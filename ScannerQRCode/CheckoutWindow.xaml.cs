using Emgu.CV;
using Emgu.CV.Structure;
using ScannerQRCode.Entities;
using ScannerQRCode.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using ZXing;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Menu;

namespace ScannerQRCode
{
    /// <summary>
    /// Interaction logic for CheckoutWindow.xaml
    /// </summary>
    public partial class CheckoutWindow : Window
    {
        private VideoCapture _captureVideo = new();
        private DispatcherTimer _timer = new();
        private bool _isQRCodeScanned = false;
        private QRCodeService _qrCodeService = new();
        private ProductService _productService = new();
        private OrderDetailService _orderDetailService = new();
        private OrderServices _orderService = new();
        private string _barCode;
        private CameraService _cameraService = new();
        private List<OrderDetail> orderDetails = new();
        private double totalPrice;

        public CheckoutWindow()
        {
            InitializeComponent();
            _captureVideo.ImageGrabbed += ProcessFrame; // Đăng ký sự kiện xử lý mỗi khung hình từ camera
            _captureVideo.Start(); // Bắt đầu lấy hình ảnh từ camera
            _timer.Interval = TimeSpan.FromMilliseconds(10000);
            _timer.Tick += (s, args) => { ProcessFrame(null, null); }; // Gọi ProcessFrame mỗi 30ms
            _timer.Start(); // Bắt đầu timer
        }

        private void ProcessFrame(object sender, EventArgs e)
        {
            using (var frame = _captureVideo.QueryFrame().ToImage<Bgr, byte>())
            {

                // Chuyển đổi hình ảnh trong luồng UI
                Dispatcher.Invoke(() =>
                {
                    var bitmap = _cameraService.ConvertToBitmap(frame);
                    var bitmapImage = _cameraService.BitmapToBitmapImage(bitmap);
                    CheckOutCam.Source = bitmapImage; // Hiển thị hình ảnh từ camera lên điều khiển Image

                    //Xử lí mã QR
                    Result result = _qrCodeService.ProcessQRCode(bitmap);
                    if (result != null)
                    {
                        _barCode = result.Text;
                        Product? product = _productService.GetProductById(_barCode);
                        if (product != null)
                        {
                            var existingOrderDetail = orderDetails.FirstOrDefault(order => order.Product.Id == product.Id);
                            if (existingOrderDetail != null)
                            {
                                if (existingOrderDetail.Quantity == product.Quantity)
                                {
                                    return;
                                }
                                // Update the quantity or other details if needed
                                existingOrderDetail.Quantity++; // Incrementing quantity as an example
                                existingOrderDetail.TotalPrice = existingOrderDetail.Quantity * existingOrderDetail.Product.Price;

                            }
                            else
                            {
                                // Add new OrderDetail if it doesn’t exist
                                orderDetails.Add(new OrderDetail { Product = product, ProductId = product.Id, Quantity = 1, TotalPrice = product.Price, Price = product.Price });
                            }
                            LoadData();

                            Task.Delay(500).Wait();
                        }
                    }
                });
            }
        }

        private void LoadData()
        {
            
            CheckoutData.ItemsSource = null;
            CheckoutData.ItemsSource = orderDetails;
            totalPrice = 0;
            foreach (OrderDetail item in orderDetails) 
            {
                totalPrice += item.TotalPrice;
            }
            TotalPriceTextBlock.Text = "Total Price: " + totalPrice.ToString();
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

        private void CheckoutData_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            {
                // Kiểm tra nếu cột chỉnh sửa là 'Unit Price' hoặc 'Quantity'
                if (e.Column.Header.ToString() == "Unit Price" || e.Column.Header.ToString() == "Quantity")
                {
                    var editedRow = e.Row.Item as OrderDetail;

                    // Lấy dòng dữ liệu được chỉnh sửa

                    if (editedRow != null)
                    {
                        Product? product = _productService.GetProductById(editedRow.Product.Id);
                        if (editedRow.Quantity > product.Quantity)
                        {
                            MessageBox.Show("Quantity is greater than product's quantity in inventory", "Invalid Quantity", MessageBoxButton.OK, MessageBoxImage.Error);
                            editedRow.Quantity = 1;
                        }
                        // Tính toán lại giá trị tổng (Total)
                        editedRow.TotalPrice = editedRow.Product.Price * editedRow.Quantity;
                        // Đảm bảo rằng Items.Refresh chỉ được gọi sau khi chỉnh sửa kết thúc
                        Dispatcher.BeginInvoke(new Action(() =>
                        {
                            CheckoutData.Items.Refresh();
                            LoadData();
                        }), System.Windows.Threading.DispatcherPriority.Background);
                    }
                }
            }
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            String id = txtSearchID.Text;
            Product? product = _productService.GetProductById(id);
            if (product != null)
            {
                OrderDetail existingOrderDetail = orderDetails.FirstOrDefault(order => order.Product.Id == product.Id);
                if (existingOrderDetail != null)
                {
                    if (existingOrderDetail.Quantity == product.Quantity)
                    {
                        return;
                    }
                    // Update the quantity or other details if needed
                    existingOrderDetail.Quantity++; // Incrementing quantity as an example
                    existingOrderDetail.TotalPrice = existingOrderDetail.Quantity * existingOrderDetail.Product.Price;

                }
                else
                {
                    // Add new OrderDetail if it doesn’t exist
                    orderDetails.Add(new OrderDetail { Product = product, ProductId = product.Id, Quantity = 1, Price = product.Price, TotalPrice = product.Price });
                }
                LoadData();
            }
        }

        private void CreateBillButton_Click(object sender, RoutedEventArgs e)
        {
            int i = 0;
            String orderId = _productService.GenerateRandomId();
            Order order = new Order()
            {
                Id = orderId,
                CreateDate = DateTime.Now,
                Price = totalPrice
            };
            _orderService.AddOrder(order);

            foreach (OrderDetail orderDetail in orderDetails)
            { 
                orderDetail.Id = ++i;
                orderDetail.OrderId = orderId;
                _orderDetailService.AddOne(orderDetail);
            }
            StopCamera();
            MessageBox.Show("Checkout successfully", "Succesful Checkout",MessageBoxButton.OK);
            this.Close();
        }
    }
}
