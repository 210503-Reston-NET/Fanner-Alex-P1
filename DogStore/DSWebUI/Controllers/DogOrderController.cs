using DSBL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DSModels;
using DSWebUI.Models;

namespace DSWebUI.Controllers
{
    public class DogOrderController : Controller
    {
        private IBuyerBL _buyerBL;
        private IStoreLocationBL _storeLocationBL;
        public IOrderBL _orderBL;
        public DogOrderController(IBuyerBL buyerBL, IStoreLocationBL storeLocationBL, IOrderBL orderBL)
        {
            _buyerBL = buyerBL;
            _storeLocationBL = storeLocationBL;
            _orderBL = orderBL;
        }
        // GET: DogOrderController
        public ActionResult Index()
        {
            return View();
        }

        // GET: DogOrderController/Details/5
        public ActionResult Details()
        {
            return View();
        }

        // GET: DogOrderController/Create
        public ActionResult Create(int id)
        {
            List<DogBuyer> dogBuyers = _buyerBL.GetAllBuyers();
            List<long> buyerIds = new List<long>();
            foreach(DogBuyer dB in dogBuyers)
            {
                buyerIds.Add(dB.PhoneNumber);
            }
            DogOrderVM dogOrderVM = new DogOrderVM();
            dogOrderVM.BuyerList = buyerIds;
            dogOrderVM.StoreId = id;
            return View(dogOrderVM);
        }

        // POST: DogOrderController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DogOrderVM dogOrderVM)
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

        // GET: DogOrderController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: DogOrderController/Edit/5
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

        // GET: DogOrderController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: DogOrderController/Delete/5
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
