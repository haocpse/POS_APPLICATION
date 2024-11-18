using Emgu.CV.Ocl;
using Microsoft.EntityFrameworkCore;
using ScannerQRCode.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ScannerQRCode.Repositories
{
    public class OrderDetailRepository
    {
        private QRContext _context;

        public List<OrderDetail> GetOrderDetailsByOrderId(string orderId)
        {
            _context = new QRContext();

            return _context.OrderDetails
                .Include(od => od.Product)  // Lấy thông tin sản phẩm
                .Where(od => od.OrderId == orderId)  // Lọc theo OrderId
                .ToList();  // Trả về danh sách OrderDetail
        }

        public OrderDetail? GetOrderDetailsById(string id)
        {
            _context = new QRContext();

            return _context.OrderDetails.Find(id);

        }

        public void AddOne(OrderDetail orderDetail)
        {
            _context = new();
            OrderDetail newOrderDetail = new()
            {
                ProductId = orderDetail.ProductId, // Chỉ cần đặt ProductId
                OrderId = orderDetail.OrderId,
                Quantity = orderDetail.Quantity,
                Price = orderDetail.Price,
                TotalPrice = orderDetail.TotalPrice
            };
            _context.OrderDetails.Add(newOrderDetail);
            _context.SaveChanges();
        }
    }
}
