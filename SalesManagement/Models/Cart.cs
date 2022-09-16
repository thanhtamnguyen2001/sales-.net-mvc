using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SalesManagement.Models
{
    public class Cart
    {
        private NorthWindDababaseDataContext dataContext = new NorthWindDababaseDataContext();

        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public decimal? UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal? Total { get { return UnitPrice * Quantity; } } 
        
        public Cart(int productId)
        {
            this.ProductID = productId;
            Product product = dataContext.Products.Single(p => p.ProductID == productId);
            this.ProductName = product.ProductName;
            this.UnitPrice = product.UnitPrice;
            this.Quantity = 1;
        }
    }
}