using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kabaxtor.Models
{
    public class CustomerCards
    {
        public int CardID { get; set; }
        public string CardName { get; set; }
        public string CardHolder { get; set; }
        public string CardNumber { get; set; }
        public string LastDate { get; set; }
        public string Cvc { get; set; }
       
    }
}