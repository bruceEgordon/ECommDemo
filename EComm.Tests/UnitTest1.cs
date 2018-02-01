using EComm.Data;
using EComm.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using Xunit;

namespace EComm.Tests
{
    public class UnitTest1
    {
        private DataContext CreateStubContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
            optionsBuilder.UseInMemoryDatabase();
            var context = new DataContext(optionsBuilder.Options);

            context.Suppliers.Add(new Supplier { Id = 1, CompanyName = "Acme" });

            context.Products.Add(new Product { Id = 1, ProductName = "Milk", UnitPrice = 2.50M, SupplierId = 1 });
            context.Products.Add(new Product { Id = 2, ProductName = "Bread", UnitPrice = 3.25M, SupplierId = 1 });
            context.Products.Add(new Product { Id = 3, ProductName = "Juice", UnitPrice = 5.75M, SupplierId = 1 });

            context.SaveChanges();
            return context;
        }

        [Fact]
        public void PassingTest()
        {
            Assert.Equal(4, (2 + 2));
        }

        [Fact]
        public void ProductDetails()
        {
            //Arrange
            var controller = new ProductController(CreateStubContext());

            //Act
            var result = controller.Detail(2);

            //Assert
            Assert.IsAssignableFrom<ViewResult>(result);
            var vr = result as ViewResult;
            Assert.IsAssignableFrom<Product>(vr.Model);
            var model = vr.Model as Product;
            Assert.Equal("Bread", model.ProductName);
        }
    }
}
