using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kabaxtor.Models;
using System.Data;
using System.Data.SqlClient;
//BU KONTROLLER SHIPPINGINFO, STATUS, DELIVERY VB SEYLERI YARATMAK ICIN KULLANILIR


namespace Kabaxtor.Controllers.Admin
{
    public class AdminOperationController : Controller
    {
        // Omer
        //string connectionString = @"data source=DESKTOP-74T5N7S\SQLSERVER2017EXP;initial catalog = KABAKSTORE; integrated security = True; MultipleActiveResultSets=True";
        // Canki
        string connectionString = @"data source =DESKTOP-4NF9LQ5\SQLEXPRESS;initial catalog = KABAKSTORE; integrated security = True; MultipleActiveResultSets=True";

        //-------------------------------------------------------------------------------------------------------------
        //shipping company uretip shipping tablosuna eklemek
        [HttpGet]
        public ActionResult CreateShippingInformation()
        {
            return View(new ShippingInformation());
        }
        [HttpPost]
        public ActionResult CreateShippingInformation(ShippingInformation shippingInformationInstance)
        {

            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {

                sqlCon.Open();
                string query = "INSERT INTO Product VALUES(@ShippingCompany,@ShippingCost,@CompanyTelephoneNumber)";
                SqlCommand sqlCommand = new SqlCommand(query, sqlCon);
                sqlCommand.Parameters.AddWithValue("@ShippingCompany", shippingInformationInstance.ShippingCompany);
                sqlCommand.Parameters.AddWithValue("@ShippingCost", shippingInformationInstance.ShippingCost);
                sqlCommand.Parameters.AddWithValue("@CompanyTelephoneNumber", shippingInformationInstance.CompanyTelephoneNumber);


                sqlCommand.ExecuteNonQuery();
            }


            return RedirectToAction("Index");

        }



        //shipping tablosundaki bilgileri guncelleme(cost ve telephone number)-guncellenmek istenen id yi secme
        [HttpGet]
        public ActionResult Edit(int id)
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


                return View(shippingInformationInstance);
            }
            else
            {

                return View();
            }
        }

        //shipping tablosundaki bilgileri guncelleme(cost ve telephone bumber)-secilen id yi guncellenmek istenen alanlara kaydetme
        [HttpPost]
        public ActionResult Edit(ShippingInformation shippingInformationInstance)
        {

            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                string query = "UPDATE Product SET ShipingCost=@ShippingCost,CompanyTelephoneNumber=@CompanyTelephoneNumber";
                SqlCommand sqlCommand = new SqlCommand(query, sqlCon);
                sqlCommand.Parameters.AddWithValue("@ShippingCost", shippingInformationInstance.ShippingCost);
                sqlCommand.Parameters.AddWithValue("@CompanyTelephoneNumber", shippingInformationInstance.CompanyTelephoneNumber);


                sqlCommand.ExecuteNonQuery();
            }



            return View();




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

            return View();
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
                


                return View();
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


        //----------------------------------------------------------------------------------------------------------
        //orders olusturmak icin kullanilir bu method create order details a gitmeli(once orders id olusmasi icin cunku orderdetailste kullanilacak)
        public ActionResult  CreateOrders(Orders ordersInstance,int customerID)
        {

            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {

                sqlCon.Open();
                string query = "INSERT INTO Orders (OrderDate,CustomerID) VALUES(@OrderDate,CustomerID)";
                SqlCommand sqlCommand = new SqlCommand(query, sqlCon);
                sqlCommand.Parameters.AddWithValue("@OrderDate", ordersInstance.OrderDate);
                sqlCommand.Parameters.AddWithValue("@CustomerID", ordersInstance.CustomerID);



                sqlCommand.ExecuteNonQuery();
            }

            return RedirectToAction("CreateOrderDetails");
        }

        //----------------------------------------------------------------------------------------------------------
        //order olusturmak icin kullanilir customerin bir urunu satin alma durumu-ship listesine gider(OrderDetails Tablosunda girdi olusur)
        [HttpGet]
        public ActionResult CreateOrderDetails() {

            return View(new Orders());
        }
        [HttpPost]
        public ActionResult CreateOrderDetails(OrderDetails orderDetailsInstance, int productID,int ordersID) {

            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {

                sqlCon.Open();
                string query = "INSERT INTO OrderDetails(OrdersID,ProductID,ProductQantity) VALUES(@OrdersID,@ProductID,@ProductQuantity)";
                SqlCommand sqlCommand = new SqlCommand(query, sqlCon);
                sqlCommand.Parameters.AddWithValue("@OrdersID",ordersID );
                sqlCommand.Parameters.AddWithValue("@ProductID", productID);
                sqlCommand.Parameters.AddWithValue("@ProductQuantity", orderDetailsInstance.ProductQuantitiy);


                sqlCommand.ExecuteNonQuery();
            }


            return RedirectToAction("Index");






        }


        [HttpGet]
        public ActionResult CreateProduct()
        {

            return View(new Product());
        }

        //Product uretip database a kaydetme
        [HttpPost]
        public ActionResult CreateProduct(Product productModelInstance)
        {

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
                productModelInstance.ProductStock = Convert.ToInt32(dataTable.Rows[0][6].ToString());
                productModelInstance.ProductPrice = Convert.ToInt32(dataTable.Rows[0][7].ToString());


                return View(productModelInstance);
            }
            else
            {

                return View();
            }
        }

        //Product tablosundaki bilgileri guncelleme(stock ve price)-secilen id ye guncellenmek istenen alanlari kaydetme
        [HttpPost]
        public ActionResult EditProduct(Product productModelInstance)
        {

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
        public ActionResult Delete(int id)
        {

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
        public ActionResult ListingProduct()
        {

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