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
    /// Interaction logic for ListOrderWindow.xaml
    /// </summary>
    public partial class ListOrderWindow : Window
    {

        private OrderServices _service = new ();
        public ListOrderWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            OrderListDataGrid.ItemsSource = _service.GetAllOrders();
        }
        private void ViewBillButton_Click(object sender, RoutedEventArgs e)
        {
            Order? selected = OrderListDataGrid.SelectedItem as Order;

            if (selected == null) 
            {
                MessageBox.Show("Please chọn one order to view BILL", "SELECT ONE", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            InvoicesWindow invoicesWindow = new InvoicesWindow();
            invoicesWindow.Order = selected;
            invoicesWindow.ShowDialog();

          
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ScanBarCodeButton_Click(object sender, RoutedEventArgs e)
        {
            ScanWindow scanWindow = new ScanWindow();
            scanWindow.IsAddProductMode = true;
            if (scanWindow.ShowDialog() == true) // Kiểm tra nếu quá trình quét thành công
            {
                string scannedCode = scanWindow.BarCodeResult; // Nhận chuỗi kết quả từ ScanWindow

                InvoicesWindow invoicesWindow = new InvoicesWindow();

                Order? order = _service.GetOrderById(scannedCode);

                if (order == null) {
                    MessageBox.Show($"Order {scannedCode} not found in system", "Invalid Scan", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                invoicesWindow.Order = order;
                invoicesWindow.ShowDialog();
            }
        }
    }
}
