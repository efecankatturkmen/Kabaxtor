using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kabaxtor.Models;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using Kabaxtor.Filters;
using Kabaxtor.ViewModels;
//BU KONTROLLER SHIPPINGINFO, STATUS, DELIVERY VB SEYLERI YARATMAK ICIN KULLANILIR


namespace Kabaxtor.Controllers
{
    public class AdminController : Controller
    {
        // Omer
        //string connectionString = @"data source=DESKTOP-74T5N7S\SQLSERVER2017EXP;initial catalog = KABAKSTORE; integrated security = True; MultipleActiveResultSets=True";
        // Canki
        string connectionString = @"data source =DESKTOP-4NF9LQ5\SQLEXPRESS;initial catalog = KABAKSTORE; integrated security = True; MultipleActiveResultSets=True";
        [AdminFilter]
        public ActionResult AdminDashboard()
        {
            return View();
        }
        //-------------------------------------------------------------------------------------------------------------
        //shipping company uretip shipping tablosuna eklemek
        [HttpGet]
        [AdminFilter]
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
        [AdminFilter]
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
        public ActionResult EditShipping(ShippingInformation shippingInformationInstance, int id)
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
        [AdminFilter]
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
        [AdminFilter]
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
        [AdminFilter]
        public ActionResult CreateStatuses()
        {


            return View(new Statuses());
        }
        [HttpPost]
        public ActionResult CreateStatuses(Statuses statusesInstance)
        {

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
        [AdminFilter]
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
        [AdminFilter]
        public ActionResult CreateProduct2()
        {

            return View();
        }

        //Product uretip database a kaydetme
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateProduct(Product productModelInstance, HttpPostedFileBase jpeg, HttpPostedFileBase jpeg2)
        {
            int tmp2;
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

                string query6 = "SELECT TOP 1 ProductID FROM Product ORDER BY ProductID DESC";
                SqlCommand sqlCommand6 = new SqlCommand(query6, sqlCon);
                tmp2 = (int)sqlCommand6.ExecuteScalar();
            }

            if (jpeg != null)
            {
                var jpegname = tmp2.ToString();
                var JpegSavePath = Path.Combine(Server.MapPath("~/Content/demos/shop/images/items/featured/") + jpegname + ".jpg");
                jpeg.SaveAs(JpegSavePath);
            }
            if (jpeg2 != null)
            {
                var jpegname = tmp2.ToString();
                var JpegSavePath = Path.Combine(Server.MapPath("~/Content/demos/shop/images/items/featured/") + jpegname + "-1.jpg");
                jpeg.SaveAs(JpegSavePath);
            }

            return RedirectToAction("CreateProduct");
        }




        //Product tablosundaki bilgileri guncelleme(stock ve price)- guncellenmek istenen id yi secme
        [HttpGet]
        [AdminFilter]
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
        [AdminFilter]
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
        [AdminFilter]
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
        //orderlari adminde listelemek icin(customername,product, price quantity ve statusunu listeler)
      
        public ActionResult ListOrderForAdmin()
        {

            DataTable dataTable = new DataTable();
            DataTable StatusTable = new DataTable();
            List<SelectListItem> StatusSelect = new List<SelectListItem>();
            List<Statuses> StatusList = new List<Statuses>();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
              

                sqlcon.Open();
                //string query = "SELECT Customer.CustomerName, Product.ProductName, Product.ProductPrice, OrderProduct.ProductQuantity, Statuses.Statuses" +
                //    "FROM Customer,Product,Orders,OrderProduct,Statuses" +
                //    "WHERE Orders.CustomerID=Customer.CustomerID AND OrderProduct.ProductID=Product.ProductID AND OrderProduct.OrdersID=Orders.OrdersID AND Orders.StatusID=Statuses.StatusID";
                string query2 = " SELECT Customer.CustomerName, Product.ProductName, Product.ProductPrice, OrderProduct.ProductQuantity, Statuses.Statuses,Orders.OrdersID FROM Customer, Product, Orders, OrderProduct, Statuses WHERE Orders.CustomerID = Customer.CustomerID AND OrderProduct.ProductID = Product.ProductID AND OrderProduct.OrdersID = Orders.OrdersID AND Orders.StatusID = Statuses.StatusID";

                SqlDataAdapter sqlData = new SqlDataAdapter(query2, sqlcon);
                sqlData.Fill(dataTable);

                string query3 = "SELECT * FROM Statuses";
                SqlDataAdapter sqlData2 = new SqlDataAdapter(query3, sqlcon);
                sqlData2.Fill(StatusTable);
                for(int i = 0; i < StatusTable.Rows.Count; i++)
                {
                    Statuses status = new Statuses();
                    status.StatusID = Convert.ToInt32( StatusTable.Rows[i][0]);
                    status.StatusName = StatusTable.Rows[i][1].ToString();
                    StatusSelect.Add(new SelectListItem() { Text = status.StatusName, Value = status.StatusID.ToString() });
                    
                }


                IndexHomeView model = new IndexHomeView
                {
                    StatusList = StatusList,
                    SelectList = StatusSelect,
                    dataTable = dataTable
                };


                return View(model);
            }
        }
        public ActionResult ChangeStatus(IndexHomeView model, int id)
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {

                sqlcon.Open();
                string query2 = "UPDATE Orders SET StatusID=@StatusID WHERE OrdersID=@OrdersID";
                SqlCommand sqlCommand2 = new SqlCommand(query2, sqlcon);
                sqlCommand2.Parameters.AddWithValue("@StatusID", model.status.StatusID);
                sqlCommand2.Parameters.AddWithValue("@OrdersID", id);
                sqlCommand2.ExecuteNonQuery();


            }


            return RedirectToAction("ListOrderForAdmin", "Admin");
        }

        public ActionResult MyCustomers()
        {
            List<Customer> CustomerList = new List<Customer>();
            DataTable dataTable = new DataTable();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {

                sqlcon.Open();
                string query2 = "SELECT * FROM Customer";
                SqlDataAdapter sqlData2 = new SqlDataAdapter(query2, sqlcon);
                sqlData2.Fill(dataTable);
         


            }
            for(int i= 0; i < dataTable.Rows.Count; i++)
            {
                Customer c = new Customer();
                c.CustomerID = Convert.ToInt32(dataTable.Rows[i][0].ToString());
                c.CustomerName = dataTable.Rows[i][1].ToString();
                c.CustomerSurname = dataTable.Rows[i][2].ToString();
                c.CustomerEmail = dataTable.Rows[i][3].ToString();
                c.CustomerNickName = dataTable.Rows[i][4].ToString();
                c.CustomerPassword = dataTable.Rows[i][5].ToString();
                c.CustomerPhone = dataTable.Rows[i][6].ToString();
                c.CustomerCreated = (DateTime)dataTable.Rows[i][7];
                c.CostomerModified = (DateTime)dataTable.Rows[i][8];
                c.LastPasswordReset = (DateTime)dataTable.Rows[i][9];
                c.CustomerActivate = (bool)dataTable.Rows[i][10];
                c.CustomerActivateGuid = dataTable.Rows[i][11].ToString();
                c.CustomerActivateGuid = dataTable.Rows[i][11].ToString();
                c.IsAdmin = (bool)dataTable.Rows[i][12];
                CustomerList.Add(c);
            }

            IndexHomeView model = new IndexHomeView
            {
                CustomerList=CustomerList
            };

            return View(model);
        }
    }
}