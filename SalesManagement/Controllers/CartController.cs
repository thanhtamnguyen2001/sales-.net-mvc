using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SalesManagement.Models;
using System.Transactions;

namespace SalesManagement.Controllers
{
    public class CartController : Controller
    {
        private NorthWindDababaseDataContext dataContext = new NorthWindDababaseDataContext();
        
        public List<Cart> GetCarts()
        {
            List<Cart> carts = Session["Cart"] as List<Cart>;
            if (carts == null)
            {
                carts = new List<Cart>();
                Session["Cart"] = carts;
            }

            return carts;
        }

        private int CountProduct()
        {
            int nums = 0;
            List<Cart> carts = Session["Cart"] as List<Cart>;
            if (carts != null)
            {
                nums = carts.Sum(c => c.Quantity);
            }

            return nums;
        }

        private decimal? Total()
        {
            decimal? total = 0;
            List<Cart> carts = Session["Cart"] as List<Cart>;
            if (carts != null)
            {
                total = carts.Sum(c => c.Total);
            }

            return total;
        }

        public ActionResult ListCarts()
        {
            List<Cart> carts = this.GetCarts();
            if (carts.Count == 0)
            {
                return RedirectToAction("Index", "Product");
            }

            ViewBag.productNumber = this.CountProduct();
            ViewBag.Total = this.Total();

            return View(carts);
        }

        public ActionResult AddToCart(int id)
        {
            List<Cart> carts = this.GetCarts();
            Cart cart = carts.Find(c => c.ProductID == id);
            
            if (cart == null)
            {
                cart = new Cart(id);
                carts.Add(cart);
            }
            else
            {
                cart.Quantity++;
            }

            return RedirectToAction("ListCarts");
        }

        public ActionResult Delete(int id)
        {
            List<Cart> carts = this.GetCarts();
            var cart = carts.Find(c => c.ProductID == id);
            
            if (cart != null)
            {
                carts.Remove(cart);
                return RedirectToAction("ListCarts");
            }
            if (carts.Count == 0)
            {
                return RedirectToAction("Index", "Product");
            }

            return RedirectToAction("ListCarts");
        }

        public ActionResult Order(FormCollection collection)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                Order order = new Order();
               
                try
                {
                    order.OrderDate = DateTime.Now;
                    dataContext.Orders.InsertOnSubmit(order);
                    dataContext.SubmitChanges();

                    List<Cart> carts = this.GetCarts();

                    foreach (var cart in carts)
                    {
                        Order_Detail order_Detail = new Order_Detail();
                        order_Detail.OrderID = order.OrderID;
                        order_Detail.ProductID = cart.ProductID;
                        order_Detail.Quantity = (short)cart.Quantity;
                        order_Detail.UnitPrice = (decimal)cart.UnitPrice;
                        order_Detail.Discount = 0;

                        dataContext.Order_Details.InsertOnSubmit(order_Detail);
                    }

                    dataContext.SubmitChanges();
                    scope.Complete();

                    Session["Cart"] = null;
                }
                catch (Exception)
                {
                    scope.Dispose();
                    return RedirectToAction("ListCarts");
                }
                return RedirectToAction("OrderDetailList", "Cart", new { id = order.OrderID} );
            }
        }

        public ActionResult OrderDetailList(int id)
        {
            ViewData["id"] = id;
            var result = dataContext.Products.Join(
                dataContext.Order_Details.Where(d => d.OrderID == id), 
                p => p.ProductID, 
                d => d.ProductID, 
               (p, d) => new OrderResult(  d.OrderID, p.ProductName, d.UnitPrice, d.Quantity ));

                

            return View(result);
        }
    }
}