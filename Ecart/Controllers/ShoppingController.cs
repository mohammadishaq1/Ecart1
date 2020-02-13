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
        private List<ShoppingCartModel> listOfShoppingCartModels;
        public ShoppingController()
        {

            objEcartDBEntities = new EcartDBEntities();
            listOfShoppingCartModels = new List<ShoppingCartModel>();
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

        [HttpPost]
        public JsonResult Index (string ItemID)
        {
            ShoppingCartModel objShoppingCartModel = new ShoppingCartModel();
            Item objItem = objEcartDBEntities.Items.Single(model => model.ItemID.ToString() == ItemID);
            if(Session["CartCounter"] != null)
            {
                listOfShoppingCartModels = Session["CartItem"] as List<ShoppingCartModel>;
            }

            if(listOfShoppingCartModels.Any(model=>model.ItemID == ItemID))
            {
                objShoppingCartModel = listOfShoppingCartModels.Single(model => model.ItemID == ItemID);
                objShoppingCartModel.Quantity = objShoppingCartModel.Quantity + 1;
                objShoppingCartModel.Total = objShoppingCartModel.Quantity * objShoppingCartModel.UnitPrice;
            }
            else
            {
                objShoppingCartModel.ItemID = ItemID;
                objShoppingCartModel.ImagePath = objItem.ImagePath;
                objShoppingCartModel.ItemName = objItem.ItemName;
                objShoppingCartModel.Quantity = 1;
                objShoppingCartModel.Total = objItem.ItemPrice;
                objShoppingCartModel.UnitPrice = objItem.ItemPrice;
                listOfShoppingCartModels.Add(objShoppingCartModel);
                
            }

            Session["CartCounter"] = listOfShoppingCartModels.Count;
            Session["CartItem"] = listOfShoppingCartModels;
            return Json(new { Success=true, Counter=listOfShoppingCartModels.Count}, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ShoppingCart()
        {
            listOfShoppingCartModels= Session["CartItem"] as List<ShoppingCartModel>;
          
            return View(listOfShoppingCartModels);
        }
        [HttpPost]
        public ActionResult AddOrder()
        {
            int OrderID = 0;
            listOfShoppingCartModels = Session["CartItem"] as List<ShoppingCartModel>;
            Order orderObj = new Order()
            {
                OrderDate = DateTime.Now,
                OrderNumber = String.Format("{0:ddmmyyyyhhmmsss}", DateTime.Now)
            };
            objEcartDBEntities.Orders.Add(orderObj);
            objEcartDBEntities.SaveChanges();
            OrderID = orderObj.OrderID;
            foreach(var item in listOfShoppingCartModels)
            {
                OrderDetail objOrderDetail = new OrderDetail();
                objOrderDetail.Total = item.Total;
                objOrderDetail.ItemID = item.ItemID;
                objOrderDetail.OrderID = OrderID;
                objOrderDetail.Quantity = item.Quantity;
                objOrderDetail.UnitPrice = item.UnitPrice;
                objEcartDBEntities.OrderDetails.Add(objOrderDetail);
                objEcartDBEntities.SaveChanges();

            }
            Session["CartItem"] = null;
            Session["CartCounter"] = null;
            return RedirectToAction("Index");
        }
    }
}