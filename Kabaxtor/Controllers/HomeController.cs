using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kabaxtor.Models;
using Kabaxtor.Models.Manager;
using Kabaxtor.ViewModels;

namespace Kabaxtor.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            DatabaseContext db = new DatabaseContext();

            HomeViewModel model = new HomeViewModel
            {
                ProductList = db.Productdb.ToList(),

            };

            return View(model);
        }
    }
}