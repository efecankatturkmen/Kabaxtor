﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kabaxtor.Models
{
    public class Customer
    {
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string CustomerSurname { get; set; }
        public int CustomerCardID { get; set; }
        public int CustomerAddressID { get; set; }
        public string CustomerEmail { get; set; }
        public DateTime CustomerCreated{ get; set; }
        public DateTime CountryModified { get; set; }
        public DateTime CustomerActivate { get; set; }
        public DateTime CustomerActivateGuid { get; set; }
        public DateTime LastPasswordReset { get; set; }


    }
}