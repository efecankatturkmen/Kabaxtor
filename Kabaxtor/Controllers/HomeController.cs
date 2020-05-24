using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using Kabaxtor.Models;
using Kabaxtor.ViewModels;

namespace Kabaxtor.Controllers
{
    public class HomeController : Controller
    {
        // Omer
        //string connectionString = @"data source=DESKTOP-74T5N7S\SQLSERVER2017EXP;initial catalog = KABAKSTORE; integrated security = True; MultipleActiveResultSets=True";
        // Canki
        string connectionString =   @"data source =DESKTOP-4NF9LQ5\SQLEXPRESS;initial catalog = KABAKSTORE; integrated security = True; MultipleActiveResultSets=True";

        // GET: Home
        [HttpGet]
        public ActionResult Index()
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection sqlcon= new SqlConnection(connectionString)) {

                sqlcon.Open();
                SqlDataAdapter sqlData = new SqlDataAdapter("SELECT * FROM Product", sqlcon);
                sqlData.Fill(dataTable);

            }
            IndexHomeView model = new IndexHomeView
            {
                dataTable = dataTable,

            };


            return View(model);
        }
     

        public ActionResult Contact()
        {
            return View();
        }

    }
}