using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using Kabaxtor.Models;


//BU KONTROLLER CUSTOMER ISLEMLERINI YAPAR  ORN. ADDRESS EKLEME, CARD EKLEME ,AD SOYAD EKLEME ,SILME VE GUNCELLEME ISLEMLERI VS.

namespace Kabaxtor.Controllers.User
{
    public class CustomerController : Controller
    {
        // Omer
        //string connectionString = @"data source=DESKTOP-74T5N7S\SQLSERVER2017EXP;initial catalog = KABAKSTORE; integrated security = True; MultipleActiveResultSets=True";
        // Canki
        string connectionString = @"data source =DESKTOP-4NF9LQ5\SQLEXPRESS;initial catalog = KABAKSTORE; integrated security = True; MultipleActiveResultSets=True";

        //customerdan bilgilerini almak icin kullanilir
        [HttpGet]
        public ActionResult CreateCustomer()
        {
            return View(new Customer());
        }
        [HttpPost]
        public ActionResult CreateCustomer(LoginModel model ) {

            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {

                sqlCon.Open();
                //customer bilgilerini alip ilgili tabloya kaydetme
                string query = "INSERT INTO Customer (CustomerName,CustomerSurname,CustomerEmail,CustomerPassword,CustomerNickName) VALUES(@CustomerName,@CustomerSurname,@CustomerEmail,@CustomerPassword,@CustomerNickName)";
                SqlCommand sqlCommand = new SqlCommand(query, sqlCon);
                sqlCommand.Parameters.AddWithValue("@CustomerName", model.Customer.CustomerName);
                sqlCommand.Parameters.AddWithValue("@CustomerSurname", model.Customer.CustomerSurname);
                sqlCommand.Parameters.AddWithValue("@CustomerEmail", model.Customer.CustomerEmail);
                sqlCommand.Parameters.AddWithValue("@CustomerPassword", model.Customer.CustomerPassword);
                sqlCommand.Parameters.AddWithValue("@CustomerNickName", model.Customer.CustomerNickName);

                sqlCommand.ExecuteNonQuery();

                //address bilgilerini alip ilgili tabloya kaydetme
                string query2 = "INSERT INTO Addresses (AddressType,AddressString,Street,Number,FlatNumber,District,Country,City) VALUES(@AddressType,@AddressString,@Street,@Number,@FlatNumber,@District,@Country,@City)";
                SqlCommand sqlCommand2 = new SqlCommand(query2, sqlCon);
                sqlCommand2.Parameters.AddWithValue("@AddressType", model.Adresses.AddressType);
                sqlCommand2.Parameters.AddWithValue("@AddressString", model.Adresses.AddressString);
                sqlCommand2.Parameters.AddWithValue("@Street", model.Adresses.Street);
                sqlCommand2.Parameters.AddWithValue("@Number", model.Adresses.Number);
                sqlCommand2.Parameters.AddWithValue("@FlatNumber", model.Adresses.FlatNumber);
                sqlCommand2.Parameters.AddWithValue("@District", model.Adresses.District);
                sqlCommand2.Parameters.AddWithValue("@Country", model.Adresses.Country);
                sqlCommand2.Parameters.AddWithValue("@City", model.Adresses.City);
                sqlCommand2.ExecuteNonQuery();


                //1.adresi secme ve customer tablosuna kaydetme
                string query6 = "SELECT TOP 1 CustomerID FROM Customer ORDER BY CustomerID DESC";
                SqlCommand sqlCommand6 = new SqlCommand(query6, sqlCon);
                int tmp2 = (int)sqlCommand6.ExecuteScalar();

                string query8 = "SELECT TOP 1 AddressID FROM Addresses ORDER BY AddressID DESC";
                SqlCommand sqlCommand8 = new SqlCommand(query8, sqlCon);
                int tmp3 = (int)sqlCommand8.ExecuteScalar();

                string query7 = "UPDATE Addresses SET CustomerID=@CustomerID WHERE AddressID=@AddressID ";
                SqlCommand sqlCommand7 = new SqlCommand(query7, sqlCon);
                sqlCommand7.Parameters.AddWithValue("@CustomerID", tmp2);
                sqlCommand7.Parameters.AddWithValue("@AddressID", tmp3);

                sqlCommand7.ExecuteScalar();
            }


            return RedirectToAction("Index");

        }
       
        //customerin bilgilerini silmek icin kullanilir(hesap silme)--id customer idsi
        public ActionResult DeleteCustomerInformation(int id) {

            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                string query = "DELETE FROM Customer WHERE CustomerID=@CustomerID";
                SqlCommand sqlCommand = new SqlCommand(query, sqlCon);
                sqlCommand.Parameters.AddWithValue("@CustomerID", id);

                sqlCommand.ExecuteNonQuery();
            }

            return View();




        }




        //customerin ikinci bir address eklemesi durumda kullanilir(id--address eklenmek istenen customerin id si )
        [HttpGet]
        public ActionResult CreateNewAddress() {

            return View(new Addresses());
        }
        [HttpPost]
        public ActionResult CreateNewAddress(Addresses addressesInstance, int id) {

            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {

                sqlCon.Open();

                //address bilgilerini alip ilgili tabloya kaydetme
                string query2 = "INSERT INTO Addresses (AddressType,AddressString,Street,Number,FlatNumber,District,Country,City) VALUES(@AddressType,@AddressString,@Street,@Number,@FlatNumber,@District,@Country,@City)";
                SqlCommand sqlCommand2 = new SqlCommand(query2, sqlCon);
                sqlCommand2.Parameters.AddWithValue("@AddressType", addressesInstance.AddressType);
                sqlCommand2.Parameters.AddWithValue("@AddressString", addressesInstance.AddressString);
                sqlCommand2.Parameters.AddWithValue("@Street", addressesInstance.Street);
                sqlCommand2.Parameters.AddWithValue("@Number", addressesInstance.Number);
                sqlCommand2.Parameters.AddWithValue("@FlatNumber", addressesInstance.FlatNumber);
                sqlCommand2.Parameters.AddWithValue("@District", addressesInstance.District);
                sqlCommand2.Parameters.AddWithValue("@Country", addressesInstance.Country);
                sqlCommand2.Parameters.AddWithValue("@City", addressesInstance.City);

                sqlCommand2.ExecuteNonQuery();

             


                //1.adresi secme ve customer tablosuna kaydetme
                string query6 = "SELECT TOP 1 AddressID FROM Addresses ORDER BY AddressID DESC";
                SqlCommand sqlCommand6 = new SqlCommand(query6, sqlCon);
                int tmp2 = (int)sqlCommand6.ExecuteScalar();

                string query7 = "INSERT INTO Customer (CustomerAddressID) VALUES(@CustomerAddressID) WHERE (CustomerID==@id)";
                SqlCommand sqlCommand7 = new SqlCommand(query7, sqlCon);
                sqlCommand7.Parameters.AddWithValue("@CustomerAddressID", tmp2);
                sqlCommand7.Parameters.AddWithValue("@id", id);


            }


            return RedirectToAction("Index");
        }

        //customerin var olan adresini guncellemesi icin kullanilir
        [HttpGet]
        public ActionResult EditCustomerAddress(int id) {

            Addresses AddressModelInstance = new Addresses();

            DataTable dataTable = new DataTable();
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                string query = "SELECT * FROM Addresses WHERE AddressID=@AddressID";
                SqlDataAdapter sqlData = new SqlDataAdapter(query, sqlCon);
                sqlData.SelectCommand.Parameters.AddWithValue("@AddressID", id);
                sqlData.Fill(dataTable);
            }
            if (dataTable.Rows.Count == 1)
            {
                AddressModelInstance.AddressID = Convert.ToInt32(dataTable.Rows[0][0].ToString());
                AddressModelInstance.AddressType = dataTable.Rows[0][1].ToString();
                AddressModelInstance.AddressString =dataTable.Rows[0][2].ToString();
                AddressModelInstance.Street = dataTable.Rows[0][3].ToString();
                AddressModelInstance.Number = dataTable.Rows[0][4].ToString();
                AddressModelInstance.FlatNumber = dataTable.Rows[0][5].ToString();
                AddressModelInstance.District = dataTable.Rows[0][6].ToString();
                AddressModelInstance.Country = dataTable.Rows[0][7].ToString();
                AddressModelInstance.City = dataTable.Rows[0][8].ToString();


                return View(AddressModelInstance);
            }
            else
            {

                return View();
            }
        }
        [HttpPost]
        public ActionResult EditCustomerAddress(Addresses AddressModelInstance)
        {

            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                string query = "UPDATE Addresses SET AddressType=@AddressType,AddressString=@AddressString, Street=@Street,Number=@Number,FlatNumber=@FlatNumber,District=@District,Country=@Country,City=@City";
                SqlCommand sqlCommand = new SqlCommand(query, sqlCon);
                sqlCommand.Parameters.AddWithValue("@AddressType", AddressModelInstance.AddressType);
                sqlCommand.Parameters.AddWithValue("@AddressString", AddressModelInstance.AddressString);
                sqlCommand.Parameters.AddWithValue("@Street", AddressModelInstance.Street);
                sqlCommand.Parameters.AddWithValue("@Number", AddressModelInstance.Number);
                sqlCommand.Parameters.AddWithValue("@FlatNumber", AddressModelInstance.FlatNumber);
                sqlCommand.Parameters.AddWithValue("@District", AddressModelInstance.District);
                sqlCommand.Parameters.AddWithValue("@Country", AddressModelInstance.Country);
                sqlCommand.Parameters.AddWithValue("@City", AddressModelInstance.City);



                sqlCommand.ExecuteNonQuery();

           
            }



            return View();
        }

        //customerin addresslerinden birini silmesi icin kullanilir
        public ActionResult DeleteAddresses(int id) {

            

            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                string query = "SELECT (CustomerAddressID) FROM Customer WHERE CustomerID=@CustomerID";
                SqlCommand sqlCommand = new SqlCommand(query, sqlCon);
                sqlCommand.Parameters.AddWithValue("@CustomerID", id);
                int tmpAddressID= sqlCommand.ExecuteNonQuery();


                string query2 = "DELETE FROM Addresses WHERE AddressID=@AddressID";
                SqlCommand sqlCommand2 = new SqlCommand(query2, sqlCon);
                sqlCommand2.Parameters.AddWithValue("@AddressID", tmpAddressID);
                sqlCommand2.ExecuteNonQuery();
                
            }

            return View();

        }


        //customer in kart bilgilerini eklemesi icin kullanilir(id--card eklenmek istenen customer in id si)
        [HttpGet]
        public ActionResult CreateCustomerCard() {

            return View(new Customer());
        }
        [HttpPost]
        public ActionResult CreateCustomerCard(CustomerCards customerCardsInstance, Customer customerInstance,int id) {

            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {

                sqlCon.Open();

                string query = "INSERT INTO Customer (CardName,CardHolder,CardNumber,LastDate,Cvc) VALUES (@CardName,@CardHolder,@CardNumber,@LastDate,Cvc)";
                SqlCommand sqlCommand = new SqlCommand(query,sqlCon);
                sqlCommand.Parameters.AddWithValue("@CardName", customerCardsInstance.CardName);
                sqlCommand.Parameters.AddWithValue("@CardHolder", customerCardsInstance.CardHolder);
                sqlCommand.Parameters.AddWithValue("@CardNumber", customerCardsInstance.CardNumber);
                sqlCommand.Parameters.AddWithValue("@LastDate", customerCardsInstance.LastDate);
                sqlCommand.Parameters.AddWithValue("@Cvc", customerCardsInstance.Cvc);
                sqlCommand.ExecuteNonQuery();


                string query2 = "INSERT INTO Customer (CustomerCardID) VALUES(@CardID) WHERE(CustomerID==@id)";
                SqlCommand sqlCommand2 = new SqlCommand(query2, sqlCon);
                sqlCommand2.Parameters.AddWithValue("@CardID",customerCardsInstance.CardID);
                sqlCommand2.Parameters.AddWithValue("@id", id);

            }

            return RedirectToAction("Index");
        }

        //customerin kayitli kartini silmesi icin kullanilir(id customer id)
        public ActionResult DeleteCustomerCards(int id) {


            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                string query = "SELECT (CustomerCardID) FROM Customer WHERE CustomerID=@CustomerID";
                SqlCommand sqlCommand = new SqlCommand(query, sqlCon);
                sqlCommand.Parameters.AddWithValue("@CustomerID", id);
                int tmpCardID = sqlCommand.ExecuteNonQuery();


                string query2 = "DELETE FROM CustomerCards WHERE CardID=@CardID";
                SqlCommand sqlCommand2 = new SqlCommand(query2, sqlCon);
                sqlCommand2.Parameters.AddWithValue("@CardID", tmpCardID);
                sqlCommand2.ExecuteNonQuery();

            }

            return View();





        }



        //----------------------------------------------------------------------------------------------------------
        //orders olusturmak icin kullanilir bu method create order details a gitmeli(once orders id olusmasi icin cunku orderdetailste kullanilacak)
        public ActionResult CreateOrders(Orders ordersInstance, int customerID)
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
        public ActionResult CreateOrderDetails()
        {

            return View(new Orders());
        }
        [HttpPost]
        public ActionResult CreateOrderDetails(OrderDetails orderDetailsInstance, int productID, int ordersID)
        {

            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {

                sqlCon.Open();
                string query = "INSERT INTO OrderDetails(OrdersID,ProductID,ProductQantity) VALUES(@OrdersID,@ProductID,@ProductQuantity)";
                SqlCommand sqlCommand = new SqlCommand(query, sqlCon);
                sqlCommand.Parameters.AddWithValue("@OrdersID", ordersID);
                sqlCommand.Parameters.AddWithValue("@ProductID", productID);
                sqlCommand.Parameters.AddWithValue("@ProductQuantity", orderDetailsInstance.ProductQuantitiy);


                sqlCommand.ExecuteNonQuery();
            }


            return RedirectToAction("Index");






        }

    }
}