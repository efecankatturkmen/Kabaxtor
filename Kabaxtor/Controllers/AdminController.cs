using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kabaxtor.Models;
using System.Data;
using System.Data.SqlClient;
using System.IO;
//BU KONTROLLER SHIPPINGINFO, STATUS, DELIVERY VB SEYLERI YARATMAK ICIN KULLANILIR


namespace Kabaxtor.Controllers
{
    public class AdminController : Controller
    {
        // Omer
        //string connectionString = @"data source=DESKTOP-74T5N7S\SQLSERVER2017EXP;initial catalog = KABAKSTORE; integrated security = True; MultipleActiveResultSets=True";
        // Canki
        string connectionString = @"data source =DESKTOP-4NF9LQ5\SQLEXPRESS;initial catalog = KABAKSTORE; integrated security = True; MultipleActiveResultSets=True";
        public ActionResult AdminDashboard()
        {
            return View();
        }
        //-------------------------------------------------------------------------------------------------------------
        //shipping company uretip shipping tablosuna eklemek
        [HttpGet]
        public ActionResult CreateShippingInformation()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateShippingInformation(ShippingInformation shippingInformationInstance)
        {

            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {

                sqlCon.Open();
                string query = "INSERT INTO ShippingInformation(ShippingCompany,ShippingCost,CompanyTelephoneNumber) VALUES(@ShippingCompany,@ShippingCost,@CompanyTelephoneNumber)";
                SqlCommand sqlCommand = new SqlCommand(query, sqlCon);
                sqlCommand.Parameters.AddWithValue("@ShippingCompany", shippingInformationInstance.ShippingCompany);
                sqlCommand.Parameters.AddWithValue("@ShippingCost", shippingInformationInstance.ShippingCost);
                sqlCommand.Parameters.AddWithValue("@CompanyTelephoneNumber", shippingInformationInstance.CompanyTelephoneNumber);


                sqlCommand.ExecuteNonQuery();
            }


            return RedirectToAction("AdminDashboard");

        }



        //shipping tablosundaki bilgileri guncelleme(cost ve telephone number)-guncellenmek istenen id yi secme
        [HttpGet]
        public ActionResult EditShipping(int id)
        {

            ShippingInformation shippingInformationInstance = new ShippingInformation();
            DataTable dataTable = new DataTable();
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                string query = "SELECT * FROM ShippingInformation WHERE ShippingID=@ShippingID";
                SqlDataAdapter sqlData = new SqlDataAdapter(query, sqlCon);
                sqlData.SelectCommand.Parameters.AddWithValue("@ShippingID", id);
                sqlData.Fill(dataTable);
            }
            if (dataTable.Rows.Count == 1)
            {
                shippingInformationInstance.ShippingID = Convert.ToInt32(dataTable.Rows[0][0].ToString());
                shippingInformationInstance.ShippingCost = Convert.ToInt32(dataTable.Rows[0][2].ToString());
                shippingInformationInstance.CompanyTelephoneNumber = dataTable.Rows[0][3].ToString();
                shippingInformationInstance.ShippingCompany = dataTable.Rows[0][1].ToString();


                return View(shippingInformationInstance);
            }
            else
            {

                return View();
            }
        }

        //shipping tablosundaki bilgileri guncelleme(cost ve telephone bumber)-secilen id yi guncellenmek istenen alanlara kaydetme
        [HttpPost]
        public ActionResult EditShipping(ShippingInformation shippingInformationInstance,int id)
        {

            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                string query = "UPDATE ShippingInformation SET ShippingCost=@ShippingCost,CompanyTelephoneNumber=@CompanyTelephoneNumber WHERE ShippingID=@ID";
                SqlCommand sqlCommand = new SqlCommand(query, sqlCon);
                sqlCommand.Parameters.AddWithValue("@ShippingCost", shippingInformationInstance.ShippingCost);
                sqlCommand.Parameters.AddWithValue("@CompanyTelephoneNumber", shippingInformationInstance.CompanyTelephoneNumber);
                sqlCommand.Parameters.AddWithValue("@ID", id);


                sqlCommand.ExecuteNonQuery();
            }



            return RedirectToAction("ListingShippingCompaniesToEdit");




        }



        //shipping tablosundaki bir girdiyi silme- silinmek istenen id yi secme
        public ActionResult DeleteShippingInfo(int id)
        {

            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                string query = "DELETE FROM ShippingInformation WHERE ShippingID=@ShippingID";
                SqlCommand sqlCommand = new SqlCommand(query, sqlCon);
                sqlCommand.Parameters.AddWithValue("@ShippingID", id);

                sqlCommand.ExecuteNonQuery();
            }

            return RedirectToAction("EditShippingCompaniesToEdit");
        }



        //listing shipping companies to choose
        [HttpGet]
        public ActionResult ListingShippingCompanies()
        {

            DataTable dataTable = new DataTable();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {

                sqlcon.Open();
                SqlDataAdapter sqlData = new SqlDataAdapter("SELECT * FROM ShippingInformation", sqlcon);
                sqlData.Fill(dataTable);
                


                return View(dataTable);
            }
        }
        [HttpGet]
        public ActionResult ListingShippingCompaniesToEdit()
        {

            DataTable dataTable = new DataTable();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {

                sqlcon.Open();
                SqlDataAdapter sqlData = new SqlDataAdapter("SELECT * FROM ShippingInformation", sqlcon);
                sqlData.Fill(dataTable);



                return View(dataTable);
            }
        }



        //------------------------------------------------------------------------------------------------------------
        //status uretip status tablosuna eklemek
        [HttpGet]
        public ActionResult CreateStatuses() {


            return View(new Statuses());
        }
        [HttpPost]
        public ActionResult CreateStatuses(Statuses statusesInstance) {

            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {

                sqlCon.Open();
                string query = "INSERT INTO ShippingInformation VALUES(@StatusName)";
                SqlCommand sqlCommand = new SqlCommand(query, sqlCon);
                sqlCommand.Parameters.AddWithValue("@StatusName", statusesInstance.StatusName);


                sqlCommand.ExecuteNonQuery();
            }


            return RedirectToAction("Index");



        }



        //statuses tablosundaki bir girdiyi silme- silinmek istenen id yi secme
        public ActionResult DeleteStatuses(int id)
        {

            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                string query = "DELETE FROM Statuses WHERE StatusID=@StatusID";
                SqlCommand sqlCommand = new SqlCommand(query, sqlCon);
                sqlCommand.Parameters.AddWithValue("@StatusID", id);

                sqlCommand.ExecuteNonQuery();
            }

            return View();
        }



        //listing statuses to choose
        [HttpGet]
        public ActionResult ListingStatuses()
        {

            DataTable dataTable = new DataTable();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {

                sqlcon.Open();
                SqlDataAdapter sqlData = new SqlDataAdapter("SELECT * FROM Statuses", sqlcon);
                sqlData.Fill(dataTable);


                return View();
            }
        }


     


      
        public ActionResult CreateProduct()
        {

            return View();
        }
        public ActionResult CreateProduct2()
        {

            return View();
        }

        //Product uretip database a kaydetme
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateProduct(Product productModelInstance, HttpPostedFileBase jpeg, HttpPostedFileBase jpeg2)
        {

            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {

                sqlCon.Open();
                string query = "INSERT INTO Product(ProductName,ProductHeight,ProductWidht,ProductStock,ProductPrice,ProductWeight,ProductComments) VALUES(@ProductName,@ProductHeight,@ProductWidht,@ProductStock,@ProductPrice,@ProductWeight,@ProductComments)";
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
            if (jpeg!=null)
            {
                var jpegname = productModelInstance.ProductID.ToString();
                var JpegSavePath = Path.Combine(Server.MapPath("~/Content/demos/shop/images/items/featured/") + jpegname + ".jpeg");
                jpeg.SaveAs(JpegSavePath);
            }
            if (jpeg2 != null)
            {
                var jpegname = productModelInstance.ProductID.ToString();
                var JpegSavePath = Path.Combine(Server.MapPath("~/Content/demos/shop/images/items/featured/") + jpegname + "-1.jpeg");
                jpeg.SaveAs(JpegSavePath);
            }

            return RedirectToAction("CreateProduct");
        }




        //Product tablosundaki bilgileri guncelleme(stock ve price)- guncellenmek istenen id yi secme
        [HttpGet]
        public ActionResult EditProduct(int id)
        {

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
                productModelInstance.ProductName = dataTable.Rows[0][1].ToString();
                productModelInstance.ProductStock = Convert.ToInt32(dataTable.Rows[0][6].ToString());
                productModelInstance.ProductPrice = Convert.ToInt32(dataTable.Rows[0][7].ToString());


                //return View(productModelInstance);
                return View(productModelInstance);
            }
            else
            {

                return RedirectToAction("ListingProductToEdit", "Admin");
            }
        }

        //Product tablosundaki bilgileri guncelleme(stock ve price)-secilen id ye guncellenmek istenen alanlari kaydetme
        [HttpPost]
        public ActionResult EditProduct(Product productModelInstance, int id)
        {

            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                string query = "UPDATE Product SET ProductStock=@ProductStock,ProductPrice=@ProductPrice WHERE ProductID=@ID";
                SqlCommand sqlCommand = new SqlCommand(query, sqlCon);
                sqlCommand.Parameters.AddWithValue("@ProductStock", productModelInstance.ProductStock);
                sqlCommand.Parameters.AddWithValue("@ProductPrice", productModelInstance.ProductPrice);
                sqlCommand.Parameters.AddWithValue("@ID", id);


                sqlCommand.ExecuteNonQuery();
            }



            return RedirectToAction("ListingProductToEdit");
        }





        //Product tablosundaki bir girdiyi silme- silinmek istenen id yi secme
        public ActionResult DeleteProduct(int id)
        {

            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                string query = "DELETE FROM Product WHERE ProductID=@ProductID";
                SqlCommand sqlCommand = new SqlCommand(query, sqlCon);
                sqlCommand.Parameters.AddWithValue("@ProductID", id);

                sqlCommand.ExecuteNonQuery();
            }

            return RedirectToAction("ListingProductToEdit");
        }




        //Databaseteki productlari listelemek icin kullanilir
     
        public ActionResult ListingProduct()
        {

            DataTable dataTable = new DataTable();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {

                sqlcon.Open();
                SqlDataAdapter sqlData = new SqlDataAdapter("SELECT * FROM Product", sqlcon);
                sqlData.Fill(dataTable);


                return View(dataTable);
            }
        }
     
        public ActionResult ListingProductToEdit()
        {

            DataTable dataTable = new DataTable();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {

                sqlcon.Open();
                SqlDataAdapter sqlData = new SqlDataAdapter("SELECT * FROM Product", sqlcon);
                sqlData.Fill(dataTable);


                return View(dataTable);
            }
        }




    }
}