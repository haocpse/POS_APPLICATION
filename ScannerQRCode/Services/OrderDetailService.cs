using ScannerQRCode.Entities;
using ScannerQRCode.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScannerQRCode.Services
{
    public class OrderDetailService
    {
        private OrderDetailRepository _repo = new OrderDetailRepository();
        private ProductRepository _productRepo = new();

        public List<OrderDetail> GetOrderDetailsByOrderId(string orderId)
        {
            return _repo.GetOrderDetailsByOrderId(orderId);
        }

        public OrderDetail? GetOrderDetailsByProductId(string id)
        {
            return _repo.GetOrderDetailsById(id);
        }

        public void AddOne(OrderDetail orderDetail)
        {
            Product? product = _productRepo.GetProductById(orderDetail.ProductId);
            product.Quantity -= orderDetail.Quantity;
            _productRepo.Update(product);
            _repo.AddOne(orderDetail);
        }
    }
}
