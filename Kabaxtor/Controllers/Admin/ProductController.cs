using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using Kabaxtor.Models;

namespace Kabaxtor.Controllers
{
    public class ProductController : Controller
    {
        string connectionString = @"data source=DESKTOP-74T5N7S\SQLSERVER2017EXP;initial catalog = KABAKSTORE; integrated security = True; MultipleActiveResultSets=True";


        //product uretip database e kaydetme
        [HttpGet]
        public ActionResult CreateProduct()
        {

            return View(new Product());
        }

        //Product uretip database a kaydetme
        [HttpPost]
        public ActionResult CreateProduct(Product productModelInstance) {

            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {

                sqlCon.Open();
                string query = "INSERT INTO Product VALUES(@ProductName,@ProductionDate,@ProductHeight,@ProductWidht,@ProductStock,@ProductPrice,@ProductWeight,@ProductComments)";
                SqlCommand sqlCommand = new SqlCommand(query, sqlCon);
                sqlCommand.Parameters.AddWithValue("@ProductName", productModelInstance.ProductName);
                sqlCommand.Parameters.AddWithValue("@ProductionDate", productModelInstance.ProductName);
                sqlCommand.Parameters.AddWithValue("@ProductHeight", productModelInstance.ProductHeight);
                sqlCommand.Parameters.AddWithValue("@ProductWidht", productModelInstance.ProductWidht);
                sqlCommand.Parameters.AddWithValue("@ProductStock", productModelInstance.ProductStock);
                sqlCommand.Parameters.AddWithValue("@ProductPrice", productModelInstance.ProductPrice);
                sqlCommand.Parameters.AddWithValue("@ProductWeight", productModelInstance.Weight);
                sqlCommand.Parameters.AddWithValue("@ProductComments", productModelInstance.ProductComments);

                sqlCommand.ExecuteNonQuery();
            }


            return RedirectToAction("Index");
        }




        //Product tablosundaki bilgileri guncelleme(stock ve price)- guncellenmek istenen id yi secme
        [HttpGet]
        public ActionResult Edit(int id) {

            Product productModelInstance = new Product();
            DataTable dataTable = new DataTable();
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                string query = "SELECT * FROM Product WHERE ProductID=@ProductID";
                SqlDataAdapter sqlData = new SqlDataAdapter(query, sqlCon);
                sqlData.SelectCommand.Parameters.AddWithValue("@ProductID", id);
                sqlData.Fill(dataTable);
            }
            if (dataTable.Rows.Count == 1)
            {
                productModelInstance.ProductID = Convert.ToInt32(dataTable.Rows[0][0].ToString());
                productModelInstance.ProductStock = Convert.ToInt32(dataTable.Rows[0][6].ToString());
                productModelInstance.ProductPrice = Convert.ToInt32(dataTable.Rows[0][7].ToString());


                return View(productModelInstance);
            }
            else {

                return View();
            }
        }

        //Product tablosundaki bilgileri guncelleme(stock ve price)-secilen id ye guncellenmek istenen alanlari kaydetme
        [HttpPost]
        public ActionResult Edit(Product productModelInstance) {

            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                string query = "UPDATE Product SET ProductStock=@ProductStock,ProductPrice=@ProductPrice";
                SqlCommand sqlCommand = new SqlCommand(query, sqlCon);
                sqlCommand.Parameters.AddWithValue("@ProductStock", productModelInstance.ProductStock);
                sqlCommand.Parameters.AddWithValue("@ProductionDate", productModelInstance.ProductPrice);
               

                sqlCommand.ExecuteNonQuery();
            }



            return View();
        }





        //Product tablosundaki bir girdiyi silme- silinmek istenen id yi secme
        public ActionResult Delete(int id) {

            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                string query = "DELETE FROM Product WHERE ProductID=@ProductID";
                SqlCommand sqlCommand = new SqlCommand(query, sqlCon);
                sqlCommand.Parameters.AddWithValue("@ProductID", id);

                sqlCommand.ExecuteNonQuery();
            }

            return View();
        }




        //Databaseteki productlari listelemek icin kullanilir
        [HttpGet]
        public ActionResult ListingProduct() {

            DataTable dataTable = new DataTable();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {

                sqlcon.Open();
                SqlDataAdapter sqlData = new SqlDataAdapter("SELECT * FROM Product", sqlcon);
                sqlData.Fill(dataTable);


                return View();
            }
        }




    }
}