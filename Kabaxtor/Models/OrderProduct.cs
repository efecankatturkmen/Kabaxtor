using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kabaxtor.Models
{
    public class OrderProduct
    {
        public int OrdersID { get; set; }
        public int ProductID { get; set; }
        public int ProductQuantitiy { get; set; }

    }
}