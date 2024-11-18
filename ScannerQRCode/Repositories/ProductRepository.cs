using ScannerQRCode.Entities;

namespace ScannerQRCode.Repositories
{

    public class ProductRepository
    {
        private QRContext _context;

        public void Add(Product product)
        {
            _context = new QRContext();
            _context.Products.Add(product);
            _context.SaveChanges();
        }

        public Product? GetProduct(string qrContent)
        {
            _context = new QRContext();
            return _context.Products.FirstOrDefault(product => product.Id == qrContent);
        }

        public List<Product> GetAll()
        {
            _context = new QRContext();
            return _context.Products.ToList();
        }

        public Product? GetProductById(string id)
        {
            _context = new QRContext();
            return _context.Products.FirstOrDefault(product => product.Id == id);
        }
        public void Delete(Product product)
        {
            _context = new QRContext();
            _context.Products.Remove(product);
            _context.SaveChanges();
        }

        public void Update(Product product)
        {
            _context = new QRContext();
            _context.Products.Update(product);
            _context.SaveChanges();
        }

    }
}
