using Kabaxtor.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kabaxtor.ViewModels
{
    public class IndexHomeView 
    {
        public DataTable dataTable { get; set; }
        public Customer customer { get; set; }
    }
}