using ScannerQRCode.Entities;
using ScannerQRCode.Repositories;

namespace ScannerQRCode.Services
{

    public class ProductService
    {
        ProductRepository ProductRepository = new();
        public void AddProduct(Product product) 
        { 
             ProductRepository.Add(product);
        }

        public Product? GetProductByQRContent(string qrContent)
        {
            return ProductRepository.GetProduct(qrContent);
        }

        public List<Product> GetAllProducts()
        {
            return ProductRepository.GetAll();
        }

        public Product? GetProductById(string id)
        {
            return ProductRepository.GetProductById(id);
        }
        public void DeleteProduct(Product product)
        {
            ProductRepository.Delete(product);
        }

        public void UpdateProduct(Product product)
        {
            ProductRepository.Update(product);
        }
        public string GenerateRandomId()
        {
            Random random = new Random();
            return random.Next(100000000, 999999999).ToString() + random.Next(100, 999).ToString();
        }


    }
}
