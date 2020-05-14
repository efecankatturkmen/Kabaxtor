using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity; //aslinda gerekli degil entitiy framework kullanirsan gerekli olur
using System.ComponentModel;


namespace Kabaxtor.Models
{
    public class Product
    {
       
        public int ProductID { get; set; }
        [DisplayName("Kabak Ismi")]
        public string ProductName { get; set; }
        [DisplayName("Tarih")]
        public DateTime ProductionDate { get; set; }
        [DisplayName("Yukseklik")]
        public int ProductHeight { get; set; }
        [DisplayName("Genislik")]
        public int ProductWidht { get; set; }
        [DisplayName(" Kabak resmi Resim")]
        public byte[] ProductImage { get; set; }
        [DisplayName("Kabak Sayisi")]
        public int ProductStock { get; set; }
        [DisplayName("Kabak Fiyati")]
        public int ProductPrice { get; set; }
        [DisplayName("Kabak Agirlik")]
        public int Weight { get; set; }
        [DisplayName("Yorum")]
        public string ProductComments { get; set; }

    }
}