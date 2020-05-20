using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kabaxtor.Models
{
    public class Addresses
    {

        public int AddressID { get; set; }
        public string AddressType { get; set; }
        public string AddressString { get; set; }
        public string Street{ get; set; }
        public string Number { get; set; }
        public string FlatNumber { get; set; }
        public string District { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public virtual Customer CustomerID { get; set; }

    }
}