using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kabaxtor.Models
{
    public class OrderDetails
    {
        public int OrdersID { get; set; }
        public int ProductID { get; set; }
        public int ProductQuantity { get; set; }

    }
}