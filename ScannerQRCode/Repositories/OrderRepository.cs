using Microsoft.EntityFrameworkCore;
using ScannerQRCode.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScannerQRCode.Repositories
{
    public class OrderRepository
    {
        private QRContext _Context;

        public List<Order> GetAllOrder()
        {
            _Context = new QRContext();
            return _Context.Orders.Include("OrderDetails").ToList();
        }

        public Order? GetOrder(string id) 
        {
            _Context = new();
            return _Context.Orders.FirstOrDefault(x => x.Id == id);
        }

        public void Update(Order obj)
        {
            _Context = new QRContext();
            _Context.Orders.Update(obj);
            _Context.SaveChanges();
        }

        public void AddOne(Order obj) 
        {
            _Context = new QRContext();
            _Context.Orders.Add(obj);
            _Context.SaveChanges();
        }

    }
}
