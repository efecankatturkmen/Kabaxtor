using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity; //aslinda gerekli degil entitiy framework kullanirsan gerekli olur

namespace Kabaxtor.Models
{
    public class Product
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public DateTime ProductionDate { get; set; }
        public int ProductHeight { get; set; }
        public int ProductWidht { get; set; }
        public byte[] ProductImage { get; set; }
        public int ProductStock { get; set; }
        public int ProductPrice { get; set; }
        public int Weight { get; set; }
        public string ProductComments { get; set; }

    }
}