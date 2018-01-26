﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EComm.Data;
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

        [Route("product/{id:int}")]
        public IActionResult Detail(int id)
        {
            var product = _dataContext.Products.Include(p => p.Supplier).SingleOrDefault(p => p.Id == id);

            if (product == null) return NotFound();

            return View(product);
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