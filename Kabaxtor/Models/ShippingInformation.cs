using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kabaxtor.Models
{
    public class ShippingInformation
    {
        public int ShippingID { get; set; }
        public string ShippingCompany { get; set; }
        public int ShippingCost { get; set; }
        public string CompanyTelephoneNumber { get; set; }

    }
}