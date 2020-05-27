using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kabaxtor.Models
{
    public class ShopCart : Controller
    {
        public Product Product { get; set; }
        public OrderDetails OrderDetails { get; set; }
    }
}