using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SalesManagement.Models;

namespace SalesManagement.Controllers
{
    public class ProductController : Controller
    {
        private readonly NorthWindDababaseDataContext dataContext = new NorthWindDababaseDataContext();

        // GET: Product
        public ActionResult Index()
        {
            var products = dataContext.Products.Select(p => p).ToList();
            return View(products);
        }

        public ActionResult Create()
        {
            ViewData["categories"] = new SelectList(dataContext.Categories, "CategoryID", "CategoryName");
            return View();
        }

        [HttpPost]
        public ActionResult Create(FormCollection collection, Product product)
        {
            var productName = collection["ProductName"];
            if (string.IsNullOrEmpty(productName))
            {
                ViewData["error"] = "Product name can't not empty";
            }
            else
            {
                product.CategoryID = int.Parse(collection["categories"]);
                dataContext.Products.InsertOnSubmit(product);
                dataContext.SubmitChanges();

                return RedirectToAction("Index");
            }

            return this.Create();
        }

        public ActionResult Edit(int id)
        {
            var product = dataContext.Products.FirstOrDefault(p => p.ProductID == id);
            return View(product);
        }

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            var product = dataContext.Products.FirstOrDefault(p => p.ProductID == id);
            product.ProductName = collection["ProductName"];
            product.SupplierID = int.Parse(collection["SupplierID"]);
            product.CategoryID = int.Parse(collection["CategoryID"]);
            product.UnitPrice = decimal.Parse(collection["UnitPrice"]);
            product.UnitsInStock = short.Parse(collection["UnitsInStock"]);

            UpdateModel(product);
            dataContext.SubmitChanges();

            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            var product = dataContext.Products.FirstOrDefault(p => p.ProductID == id);
            return View(product);
        }

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            var product = dataContext.Products.FirstOrDefault(p => p.ProductID == id);
            dataContext.Products.DeleteOnSubmit(product);

            return RedirectToAction("Index");
        }

        public ActionResult Details(int id)
        {
            var product = dataContext.Products.FirstOrDefault(p => p.ProductID == id);
            return View(product);
        }

        


    }
}