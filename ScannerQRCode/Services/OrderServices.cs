using ScannerQRCode.Entities;
using ScannerQRCode.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScannerQRCode.Services
{
    public class OrderServices
    {
        private OrderRepository _repo = new OrderRepository();

        public List<Order> GetAllOrders()
        {
            return _repo.GetAllOrder();
        }


        public void AddOrder(Order order) 
        { 
            _repo.AddOne(order);
        }


        public Order? GetOrderById(string id)
        {
            return _repo.GetOrder(id);
        }

    }
}
