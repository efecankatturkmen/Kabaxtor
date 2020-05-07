using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kabaxtor.Models
{
    public class Delivery
    {
        public int DeliveryID { get; set; }
        public DateTime EstimatedDeliveryDate { get; set; }
        public string DeliveryCode { get; set; }
        public string DeliveryStatus { get; set; }

    }
}