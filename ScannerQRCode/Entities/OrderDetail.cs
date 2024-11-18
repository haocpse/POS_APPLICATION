using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ScannerQRCode.Entities
{
    public class OrderDetail
    {
        [Key]
        public int Id { get; set; }

        // Thiết lập quan hệ với Order
        public string OrderId { get; set; }

        [ForeignKey("OrderId")]
        public Order Order { get; set; }

        // Thiết lập quan hệ với Product
        public string ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        public int Quantity { get; set; }
        public double Price { get; set; }
        public double TotalPrice { get; set; } // Tính toán TotalPrice
    }
}
