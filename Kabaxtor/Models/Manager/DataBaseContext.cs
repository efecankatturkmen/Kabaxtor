using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;


namespace Kabaxtor.Models.Manager
{

    public class DatabaseContext : DbContext
    {
        public DbSet<Addresses> Addressesdb { get; set; }
        public DbSet<Customer> Customerdb { get; set; }
        public DbSet<CustomerCards> CustomerCardsdb { get; set; }
        public DbSet<Delivery> Deliverydb { get; set; }
        public DbSet<OrderProduct> OrderProductdb { get; set; }
        public DbSet<Orders> Ordersdb { get; set; }
        public DbSet<Product> Productdb { get; set; }
        public DbSet<ShippingInformation> ShippingInformationdb { get; set; }
        public DbSet<Statuses> Statusesdb { get; set; }
        public DbSet<User> Userdb { get; set; }
        public DbSet<ZipCode> ZipCodedb { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<DbContext>(null);
            base.OnModelCreating(modelBuilder);
        }

        //public DatabaseContext()
        //{

        //Database.SetInitializer(new DataBaseMaker());
        //    Database.SetInitializer(new MigrateDatabaseToLatestVersion<DatabaseContext, e_learining_udemy.Migrations.Configuration>());
        //}
        //public DatabaseContext()
        //{
        //    Database.SetInitializer(new MigrateDatabaseToLatestVersion<DatabaseContext, e_learining_udemy.Migrations.Configuration>());
        //}
    }
}