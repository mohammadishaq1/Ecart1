using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ecart.ViewModel
{
    public class OrderModel
    {
        public int OrderID { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderNumber { get; set; }
    }
}