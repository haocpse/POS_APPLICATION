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

namespace ScannerQRCode
{
    /// <summary>
    /// Interaction logic for InvoicesWindow.xaml
    /// </summary>
    public partial class InvoicesWindow : Window
    {

        public Order Order { get; set; }
        private CameraService _cameraService = new CameraService();

        private OrderDetailService _service = new OrderDetailService(); 
        public InvoicesWindow()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

         

            List<OrderDetail>? orderDetailList = _service.GetOrderDetailsByOrderId(Order.Id);

            if (orderDetailList == null)
            {
                MessageBox.Show("Please chọn one order to view BILL", "SELECT ONE", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            OrderDetailListBox.ItemsSource = orderDetailList;


            int barcodeWidth = (int)BarcodeImage.Width;
            int barcodeHeight = (int)BarcodeImage.Height;
            BarcodeImage.Source = _cameraService.ConverIdToBarcodeImage(Order.Id, barcodeWidth, barcodeHeight);


            TotalPriceTextBlock.Text = $"Total Price: {Order.Price.ToString()}" ;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
