using Ecart.Models;
using Ecart.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ecart.Controllers
{
    public class ShoppingController : Controller
    {

        private EcartDBEntities objEcartDBEntities;
        public ShoppingController()
        {

            objEcartDBEntities = new EcartDBEntities();
        }
        // GET: Shopping
        public ActionResult Index()
        {
            IEnumerable<ShoppingViewModel> listOfShoppingViewModels = (from objItem in objEcartDBEntities.Items
                                                                       join
                                                                       objCate in objEcartDBEntities.Categories
                                                                       on objItem.CategoryID equals objCate.CategoryID
                                                                       select new ShoppingViewModel()
                                                                       {
                                                                           ImagePath=objItem.ImagePath,
                                                                           ItemName=objItem.ItemName,
                                                                           Description=objItem.Description,
                                                                           ItemPrice=objItem.ItemPrice,
                                                                           ItemID=objItem.ItemID,
                                                                           Category=objCate.CategoryName,
                                                                           ItemCode=objItem.ItemCode
                                                                       }

                                                                       ).ToList();
            return View(listOfShoppingViewModels);
        }
    }
}