using Kabaxtor.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kabaxtor.ViewModels
{
    public class IndexHomeView 
    {
        public DataTable dataTable { get; set; }
        public Customer customer { get; set; }
        public List<Customer> CustomerList { get; set; }
        public CustomerCards customercard { get; set; }
        public Addresses  addresses { get; set; }
        public Delivery delivery { get; set; }
        public OrderDetails orderDetails { get; set; }
        public List<ShopCart> ShopCart { get; set; }
        public List<Product> ProductList { get; set; }
        public List<Statuses> StatusList { get; set; }
        public List<Addresses> AdressList { get; set; }
        public Product product { get; set; }
        public Orders orders { get; set; }
        public Statuses status { get; set; }
        public ShippingInformation shippingInformation { get; set; }
        public List<SelectListItem> SelectList { get; set; }

    }
}