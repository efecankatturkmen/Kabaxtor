using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kabaxtor.Models
{
    public class LoginModel 
    {
        // GET: LoginModel
       public Customer Customer { get; set; }
        public Addresses Adresses { get; set; }
    }
}