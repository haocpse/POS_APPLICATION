using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScannerQRCode.Entities
{
    public class Order
    {
        public string Id { get; set; }
        public double Price { get; set; }
        public DateTime CreateDate { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }

    }
}
