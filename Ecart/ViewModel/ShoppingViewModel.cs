using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ecart.ViewModel
{
    public class ShoppingViewModel
    {
        public Guid ItemID { get; set; }
        public string ItemName { get; set; }
        public decimal ItemPrice { get; set; }
        public string ImagePath { get; set; }
        public string Description { get; set; }
        public string ItemCode { get; set; }
        public string Category { get; set; }
    }
}