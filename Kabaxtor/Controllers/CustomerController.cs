using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using Kabaxtor.Models;
using Kabaxtor.ViewModels;
using Kabaxtor.Filters;


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
        [AuthFilter]
        public ActionResult CustomerDashboard()
        {
          
            Customer customer = new Customer();
            customer = Session["customer"] as Customer;
            DataTable dataTable = new DataTable();
            List<ShopCart> SCart = new List<ShopCart>();
            
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {

                sqlcon.Open();
                SqlDataAdapter sqlData = new SqlDataAdapter("SELECT * FROM Product", sqlcon);
                sqlData.Fill(dataTable);
              

            }
            //DataTable ProductTable = new DataTable();
            //using (SqlConnection sqlCon2 = new SqlConnection(connectionString))
            //{
               
            //    sqlCon2.Open();                           
            //    string query = "SELECT * FROM Orders WHERE CustomerID=@CustomerID";
            //    SqlDataAdapter sqlData = new SqlDataAdapter(query, sqlCon2);
            //    sqlData.SelectCommand.Parameters.AddWithValue("@CustomerID", customer.CustomerID);
            //    sqlData.Fill(ProductTable);
            //}
            DataTable OrderProduct = new DataTable();
            using (SqlConnection sqlCon2 = new SqlConnection(connectionString))
            {

                sqlCon2.Open();
                string query = "SELECT OrderProduct.OrdersID,OrderProduct.ProductID,OrderProduct.ProductQuantity FROM OrderProduct,Orders WHERE Orders.CustomerID=@CustomerID AND OrderProduct.OrdersID=Orders.OrdersID";
                SqlDataAdapter sqlData = new SqlDataAdapter(query, sqlCon2);
                sqlData.SelectCommand.Parameters.AddWithValue("@CustomerID", customer.CustomerID);
                sqlData.Fill(OrderProduct);
            }
          

            for (int i = 0; i< OrderProduct.Rows.Count;i++)
            {
                int price = 0; ;
                string name="";
                OrderDetails cart = new OrderDetails();
                Product product = new Product();
                cart.OrdersID = Convert.ToInt32(OrderProduct.Rows[i][0]);
                cart.ProductID = Convert.ToInt32(OrderProduct.Rows[i][1]);
                cart.ProductQuantity = Convert.ToInt32(OrderProduct.Rows[i][2]);
                using (SqlConnection sqlCon3 = new SqlConnection(connectionString))
                {
                    sqlCon3.Open();
                    string query8 = "SELECT ProductName FROM Product WHERE ProductID=@ProductID";
                    SqlCommand sqlCommand8 = new SqlCommand(query8, sqlCon3);
                    sqlCommand8.Parameters.AddWithValue("@ProductID", cart.ProductID);
                    name = (string)sqlCommand8.ExecuteScalar();

                    string query9 = "SELECT ProductPrice FROM Product WHERE ProductID=@ProductID";
                    SqlCommand sqlCommand9 = new SqlCommand(query9, sqlCon3);
                    sqlCommand9.Parameters.AddWithValue("@ProductID", cart.ProductID);
                    price = (int)sqlCommand9.ExecuteScalar();
                }
                    product.ProductName = name;
                    product.ProductPrice = price;
                    SCart.Add(new ShopCart { OrderDetails = cart, Product=product});
            }



            IndexHomeView model = new IndexHomeView
            {
                dataTable = dataTable,
                customer = customer,
                ShopCart = SCart
            };


            return View(model);
        }
        [HttpGet]
        public ActionResult CreateCustomer()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateCustomer(LoginModel model ) {

            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                string check = "SELECT CustomerID FROM Customer WHERE CustomerNickName=@CustomerNickName";
                SqlCommand sqlCommand4 = new SqlCommand(check, sqlCon);
                sqlCommand4.Parameters.AddWithValue("@CustomerNickName", model.Customer.CustomerNickName);
                
                sqlCommand4.ExecuteScalar();
                string check2 = "SELECT CustomerID FROM Customer WHERE CustomerEmail=@CustomerEmail ";
                SqlCommand sqlCommand5 = new SqlCommand(check2, sqlCon);
                sqlCommand5.Parameters.AddWithValue("@CustomerEmail", model.Customer.CustomerEmail);
                sqlCommand5.ExecuteScalar();
                if (check == null && check2==null)
                {
                    
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
                if (check != null)
                {

                    ViewBag.Result = "Bu Email kullanilmaktadir";
                    ViewBag.Status = "warning";
                    return View();
                }
                if (check2 != null)
                {

                    ViewBag.Result = "Bu Email kullanilmaktadir";
                    ViewBag.Status = "warning";
                    return View();
                }
            }


            return RedirectToAction("CreateCustomer", "Home");

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
        [HttpPost]
        public ActionResult SignIn(IndexHomeView model)
        {
            int tmp;
            Customer c = new Customer();
            DataTable dataTable = new DataTable();
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                
                sqlCon.Open();
                string query = "SELECT CustomerID FROM Customer WHERE (CustomerNickName=@CustomerNickName AND CustomerPassword=@CustomerPassword )";
                SqlCommand sqlCommand = new SqlCommand(query, sqlCon);
                sqlCommand.Parameters.AddWithValue("@CustomerNickName", model.customer.CustomerNickName);
                sqlCommand.Parameters.AddWithValue("@CustomerPassword", model.customer.CustomerPassword);
                tmp = (int)sqlCommand.ExecuteScalar();
                // if ekle boyle bir kullanici yok ise yada sifre yanlis ise


               
            }
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                string query = "SELECT * FROM Customer WHERE CustomerID=@CustomerID";
                SqlDataAdapter sqlData = new SqlDataAdapter(query, sqlCon);
                sqlData.SelectCommand.Parameters.AddWithValue("@CustomerID", tmp);
                sqlData.Fill(dataTable);

            }
            if (dataTable.Rows.Count == 1)
            {
                c.CustomerID = Convert.ToInt32(dataTable.Rows[0][0].ToString());
                c.CustomerName = dataTable.Rows[0][1].ToString();
                c.CustomerSurname = dataTable.Rows[0][2].ToString();
                c.CustomerEmail = dataTable.Rows[0][3].ToString();
                c.CustomerNickName = dataTable.Rows[0][4].ToString();
                c.CustomerPassword = dataTable.Rows[0][5].ToString();
                c.CustomerPhone = dataTable.Rows[0][6].ToString();
                c.CustomerCreated = (DateTime)dataTable.Rows[0][7];
                c.CostomerModified = (DateTime)dataTable.Rows[0][8];
                c.LastPasswordReset = (DateTime)dataTable.Rows[0][9];
                c.CustomerActivate = (bool)dataTable.Rows[0][10];
                c.CustomerActivateGuid = dataTable.Rows[0][11].ToString();



                //return View(productModelInstance);

            }
            Session["customer"] = c;

            return RedirectToAction("CustomerDashboard","Customer");
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
        public ActionResult CreateCustomerCard(CustomerCards customerCardsInstance) {
            Customer customerInstance = new Customer();
            customerInstance = Session["Customer"] as Customer;
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {

                sqlCon.Open();

                string query = "INSERT INTO CustomerCards (CardName,CardHolder,CardNumber,LastDate,Cvc,CustomerID) VALUES (@CardName,@CardHolder,@CardNumber,@LastDate,Cvc,@CustomerID)";
                SqlCommand sqlCommand = new SqlCommand(query,sqlCon);
                sqlCommand.Parameters.AddWithValue("@CardName", customerCardsInstance.CardName);
                sqlCommand.Parameters.AddWithValue("@CardHolder", customerCardsInstance.CardHolder);
                sqlCommand.Parameters.AddWithValue("@CardNumber", customerCardsInstance.CardNumber);
                sqlCommand.Parameters.AddWithValue("@LastDate", customerCardsInstance.LastDate);
                sqlCommand.Parameters.AddWithValue("@Cvc", customerCardsInstance.Cvc);
                sqlCommand.Parameters.AddWithValue("@CustomerID",customerInstance.CustomerID);
                sqlCommand.ExecuteNonQuery();
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
       
        public ActionResult FastCreateOrder(int id)
        {
            Customer customerInstance = new Customer();
            customerInstance = Session["Customer"] as Customer;
            DateTime date = DateTime.Now;
            int quantity = 1;
            int orderid = 0 ;
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {

                sqlCon.Open();
                string query = "INSERT INTO Orders (OrderDate,CustomerID,StatusID) VALUES(@OrderDate,@CustomerID,@StatusID)";
                SqlCommand sqlCommand = new SqlCommand(query, sqlCon);
                sqlCommand.Parameters.AddWithValue("@OrderDate", date);
                sqlCommand.Parameters.AddWithValue("@CustomerID",customerInstance.CustomerID);
                sqlCommand.Parameters.AddWithValue("@StatusID", 1);
                sqlCommand.ExecuteNonQuery();


                DataTable OrderProductTable = new DataTable();
                
                string query12 = "SELECT * FROM OrderProduct,Orders WHERE Orders.CustomerID=@CustomerID AND OrderProduct.OrdersID=Orders.OrdersID AND ProductID=@ProductID";
                SqlDataAdapter sqlData = new SqlDataAdapter(query12, sqlCon);
                sqlData.SelectCommand.Parameters.AddWithValue("@CustomerID", customerInstance.CustomerID);
                sqlData.SelectCommand.Parameters.AddWithValue("@ProductID", id);
                sqlData.Fill(OrderProductTable);


                if (OrderProductTable.Rows.Count >= 1)
                {
                    for(int i = 0;i< OrderProductTable.Rows.Count; i++)
                    {
                        quantity = Convert.ToInt32(OrderProductTable.Rows[i][2]);
                        orderid = Convert.ToInt32(OrderProductTable.Rows[i][0]);
                    }
                    

                    string query22 = "UPDATE OrderProduct SET ProductQuantity=@ProductQuantity WHERE  ProductID=@ProductID AND OrdersID=@OrdersID";
                    SqlCommand sqlCommand15 = new SqlCommand(query22, sqlCon);
                    sqlCommand15.Parameters.AddWithValue("@ProductID", id);
                    sqlCommand15.Parameters.AddWithValue("@ProductQuantity", quantity+1);
                    sqlCommand15.Parameters.AddWithValue("@OrdersID", orderid);
                    sqlCommand15.ExecuteNonQuery();
                }
                else
                {
                    string query5 = "SELECT TOP 1 OrdersID FROM Orders ORDER BY OrdersID DESC";
                    SqlCommand sqlCommand5 = new SqlCommand(query5, sqlCon);
                    int tmp2 = (int)sqlCommand5.ExecuteScalar();

                    string check = "SELECT OrderProduct.OrdersID,OrderProduct.ProductID,OrderProduct.ProductQuantity FROM OrderProduct,Orders WHERE Orders.CustomerID=@CustomerID AND OrderProduct.OrdersID=Orders.OrdersID";
                    SqlDataAdapter sqlData33 = new SqlDataAdapter(check, sqlCon);
                    sqlData33.SelectCommand.Parameters.AddWithValue("@CustomerID", customerInstance.CustomerID);


                    string query2 = "INSERT INTO OrderProduct  (OrdersID,ProductID,ProductQuantity) VALUES(@OrdersID,@ProductID,@ProductQuantity)";
                    SqlCommand sqlCommand2 = new SqlCommand(query2, sqlCon);
                    sqlCommand2.Parameters.AddWithValue("@ProductID", id);
                    sqlCommand2.Parameters.AddWithValue("@ProductQuantity", quantity);
                    sqlCommand2.Parameters.AddWithValue("@OrdersID", tmp2);
                    sqlCommand2.ExecuteNonQuery();
                }
               

                string query3 = "SELECT ProductStock FROM Product WHERE ProductID=@ProductID";
                SqlCommand sqlCommand3 = new SqlCommand(query3, sqlCon);
                sqlCommand3.Parameters.AddWithValue("@ProductID", id);
                int tmp = (int)sqlCommand3.ExecuteScalar();

                tmp = tmp - 1;

                string query4 = "UPDATE Product SET ProductStock=@ProductStock WHERE ProductID=@ProductID";
                SqlCommand sqlCommand4 = new SqlCommand(query4, sqlCon);
                sqlCommand4.Parameters.AddWithValue("@ProductID", id);
                sqlCommand4.Parameters.AddWithValue("@ProductStock", tmp);
                sqlCommand4.ExecuteNonQuery();

            }

            return RedirectToAction("CustomerDashboard","Customer");
        }

        ////listing shipping companies to choose
        //[HttpGet]
        //public ActionResult ListingShippingCompaniesToSelect(int id)
        //{

        //    DataTable dataTable = new DataTable();
        //    using (SqlConnection sqlcon = new SqlConnection(connectionString))
        //    {

        //        sqlcon.Open();
        //        SqlDataAdapter sqlData = new SqlDataAdapter("SELECT * FROM ShippingInformation", sqlcon);
        //        sqlData.Fill(dataTable);



        //        return View(dataTable);
        //    }
        //}
        //[HttpGet]
        //public ActionResult Payment() {

        //    DataTable dataTable = new DataTable();

        //    Customer customerInstance = new Customer();
        //    customerInstance = Session["Customer"] as Customer;

        //    using (SqlConnection sqlCon = new SqlConnection(connectionString))
        //    {
        //        sqlCon.Open();
        //        string check = "SELECT CardID FROM Customer WHERE CustomerID=@CustomerID";
        //        SqlCommand sqlCommand = new SqlCommand(check, sqlCon);
        //        sqlCommand.Parameters.AddWithValue("@CustomerID", customerInstance.CustomerID);
        //        sqlCommand.ExecuteNonQuery();

        //        if (check == null)
        //        {
        //            return RedirectToAction("CreateCurtomerCard");
        //        }
        //        else {

        //            SqlDataAdapter sqlData = new SqlDataAdapter("SELECT * FROM CustomerCard", sqlCon);
        //            sqlData.Fill(dataTable);

        //            return View(dataTable);
        //        }
        //    }
    
        //}

        //[HttpPost]
        //public ActionResult Payment (int id) {

        //    using (SqlConnection sqlCon = new SqlConnection(connectionString))
        //    {

        //        sqlCon.Open();
        //        string query = "INSERT INTO Orders (OrderDate,CustomerID) VALUES(@OrderDate,@CustomerID)";
        //        SqlCommand sqlCommand = new SqlCommand(query, sqlCon);
        //        sqlCommand.Parameters.AddWithValue("@OrderDate", ordersInstance.OrderDate);
        //        sqlCommand.Parameters.AddWithValue("@CustomerID", customerInstance.CustomerID);
        //        sqlCommand.ExecuteNonQuery();



        //    }



        //        return View();
        //}

    }
}