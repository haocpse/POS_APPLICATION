using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScannerQRCode.Entities
{
    public class Product
    {
        public string Id { get; set; }
        public string ProductName { get; set; }
        public double Price { get; set; }

        public int Quantity { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }

    }
}
