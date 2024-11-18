using ScannerQRCode.Entities;
using ScannerQRCode.Services;
using System.Windows;

namespace ScannerQRCode
{
    /// <summary>
    /// Interaction logic for ProductList.xaml
    /// </summary>
    public partial class ProductList : Window
    {
        private ProductService _productService = new ProductService();

        public ProductList()
        {
            InitializeComponent();
        }


        private void LoadProducts()
        {
            ProductsDataGrid.ItemsSource = _productService.GetAllProducts();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            
            ProductWindow productWindow = new();
            productWindow.ShowDialog();
            LoadProducts();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadProducts();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            Product? selectedProduct = ProductsDataGrid.SelectedItem as Product;
            if (selectedProduct == null)
            {

                MessageBox.Show("You have to choose a product before update", "Select One", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var productWindow = new ProductWindow();
            productWindow.product = selectedProduct;
            productWindow.ShowDialog();
            LoadProducts();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            Product? selectedProduct = ProductsDataGrid.SelectedItem as Product;
            if (selectedProduct == null)
            {

                MessageBox.Show("You have to choose a product before update", "Select One", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this product?", "Delete Product", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.No)
            {
                return;
            }
            _productService.DeleteProduct(selectedProduct);
            LoadProducts();
        }

        private void ScanBarcode_Click(object sender, RoutedEventArgs e)
        {
            ScanWindow scanWindow = new ScanWindow();
            scanWindow.ShowDialog();
            LoadProducts();
        }

        private void ViewOrderListButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            ListOrderWindow listOrderWindow = new ListOrderWindow();
            listOrderWindow.ShowDialog();
            this.Show();
            
        }

        private void CheckOutButton_Click(object sender, RoutedEventArgs e)
        {
            CheckoutWindow checkoutWindow = new CheckoutWindow();
            checkoutWindow.ShowDialog();
            LoadProducts();
        }
    }
}
