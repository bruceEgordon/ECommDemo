using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EComm.Data;
using EComm.Web.Models;
using EComm.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EComm.Web.Controllers
{
    public class ProductController : Controller
    {
        private DataContext _dataContext;

        public ProductController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Policy = "AdminsOnly")]
        [Route("product/{id:int}")]
        public IActionResult Detail(int id)
        {
            var product = _dataContext.Products.Include(p => p.Supplier).SingleOrDefault(p => p.Id == id);

            if (product == null) return NotFound();

            return View(product);
        }

        [HttpPost]
        public IActionResult AddToCart(int id, int quantity)
        {
            var product = _dataContext.Products.SingleOrDefault(p => p.Id == id);
            var totalCost = quantity * product.UnitPrice;

            string message = $"You added {product.ProductName} " +
                $"(x {quantity}) to your cart " +
                $"at a total cost of {totalCost:C}.";

            var cart = ShoppingCart.GetFromSession(HttpContext.Session);
            var lineItem = cart.LineItems.SingleOrDefault(item => item.Product.Id == id);
            if (lineItem != null)
            {
                lineItem.Quantity += quantity;
            }
            else
            {
                cart.LineItems.Add(new ShoppingCart.LineItem
                {
                    Product = product,
                    Quantity = quantity
                });
            }
            ShoppingCart.StoreInSession(cart, HttpContext.Session);

            return PartialView("_AddedToCart", message);
        }

        public IActionResult Cart()
        {
            var cart = ShoppingCart.GetFromSession(HttpContext.Session);
            var cvm = new CartViewModel() { Cart = cart };
            return View(cvm);
        }

        [HttpPost]
        public IActionResult Checkout(CartViewModel cvm)
        {
            if (!ModelState.IsValid)
            {
                cvm.Cart = ShoppingCart.GetFromSession(HttpContext.Session);
                return View("Cart", cvm);
            }
            //TODO: Charge the customer's card and record the order
            HttpContext.Session.Clear();
            return View("ThankYou");
        }

        [HttpGet("api/products")]
        public IActionResult Get()
        {
            var products = _dataContext.Products.ToList();
            return new ObjectResult(products);
        }

        [HttpGet("product/{id:int}")]
        public IActionResult Get(int id)
        {
            var product = _dataContext.Products.SingleOrDefault(p => p.Id == id);
            if (product == null) return NotFound();

            return new ObjectResult(product);
        }

        [HttpPut("api/product/{id:int}")]
        public IActionResult Put(int id, [FromBody]Product product)
        {
            if(product == null || product.Id != id){ return BadRequest(); }

            var existing = _dataContext.Products.SingleOrDefault(p => p.Id == id);

            if (existing == null) return NotFound();

            existing.ProductName = product.ProductName;
            existing.UnitPrice = product.UnitPrice;
            existing.Package = product.Package;
            existing.IsDiscontinued = product.IsDiscontinued;
            existing.SupplierId = product.SupplierId;
            _dataContext.SaveChanges();

            return new NoContentResult();
        }
    }

    public class ProductsList: ViewComponent
    {
        private DataContext _dataContext;

        public ProductsList(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public IViewComponentResult Invoke()
        {
            var model = _dataContext.Products.ToList();
            return View("~/Views/Shared/_ProductList.cshtml", model);
        }
    }
}