using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SalesManagement.Models
{
    public class OrderResult
    {
        public int OrderID { get; set; }
        public string ProductName { get; set; }
        public decimal? UnitPrice { get; set; }
        public short Quantity { get; set; }

        public OrderResult(int orderID, string productName, decimal? unitPrice, short quantity)
        {
            OrderID = orderID;
            ProductName = productName;
            UnitPrice = unitPrice;
            Quantity = quantity;
        }
    }
}