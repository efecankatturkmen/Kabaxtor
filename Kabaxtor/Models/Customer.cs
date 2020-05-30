using System;
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
        public string CustomerNickName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPassword { get; set; }
        public string CustomerPhone { get; set; }
        public DateTime CustomerCreated{ get; set; }
        public DateTime CostomerModified { get; set; }
        public bool CustomerActivate { get; set; }
        public string CustomerActivateGuid { get; set; }
        public DateTime LastPasswordReset { get; set; }
        public bool IsAdmin { get; set; }


    }
}