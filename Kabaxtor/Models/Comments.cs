using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kabaxtor.Models
{
    public class Comments
    {
        public int ID { get; set; }
        public virtual Product Product { get; set; }
        public virtual Customer Customer { get; set; }
        public string Comment { get; set; }
        public int Point { get; set; }
    }
}