using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kabaxtor.Models
{
    public class Orders
    {
        public int OrdersID { get; set; }
        public int ShippingID { get; set; }
        public int DeliveryID { get; set; }
        public int CustomerID { get; set; }
        public DateTime OrderDate { get; set; }
        public int StatusID { get; set; }

    }
}