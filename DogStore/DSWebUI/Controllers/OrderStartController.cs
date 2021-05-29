using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DSModels;
using DSBL;
using DSWebUI.Models;

namespace DSWebUI.Controllers
{
    public class OrderStartController : Controller
    {
        private IStoreLocationBL _storeLocationBL;
        private IBuyerBL _buyerBL;
        public OrderStartController(IBuyerBL buyerBL, IStoreLocationBL storeLocationBL)
        {
            _storeLocationBL = storeLocationBL;
            _buyerBL = buyerBL;
        }
        // GET: OrderStartController
        public ActionResult Index()
        {
            return View();
        }

        // GET: OrderStartController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: OrderStartController/Create
        public ActionResult Create()
        {
            List<OrderStartVM> orderStarts = new List<OrderStartVM>();
            List<DogBuyer> dogBuyers = _buyerBL.GetAllBuyers();
            List<StoreLocation> storeLocations = _storeLocationBL.GetAllStoreLocations();
            foreach(DogBuyer buyer in dogBuyers)
            {
                OrderStartVM orderStartVM = new OrderStartVM();
                orderStartVM.BuyerAddress = buyer.Address;
                orderStartVM.BuyerName = buyer.Name;
                orderStartVM.BuyerId = buyer.PhoneNumber;
                orderStartVM.BuyPointer = -1;
                orderStarts.Add(orderStartVM);
            }
            
            foreach(StoreLocation storeLocation in storeLocations)
            {
                OrderStartVM orderStartVM = new OrderStartVM();
                orderStartVM.StoreAddress = storeLocation.Address;
                orderStartVM.StoreName = storeLocation.Location;
                orderStartVM.StoreId = storeLocation.Id;
                orderStartVM.BuyPointer = 1;
                orderStarts.Add(orderStartVM);
            }
            orderStarts[0].TotLength = orderStarts.Count();
            return View(orderStarts);
        }

        // POST: OrderStartController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IEnumerable<DSWebUI.Models.OrderStartVM>
 collection)
        {
            try
            {
                //OrderStartVM o = collection.Last();
                //var z = o.BuyerId;
                var x = Request.Form["BuyerStart"];
                var y = Request.Form["StoreStart"];
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: OrderStartController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: OrderStartController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: OrderStartController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: OrderStartController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
