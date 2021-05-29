using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DSBL;
using DSModels;
using DSWebUI.Models;
namespace DSWebUI.Controllers
{
    public class StoreLocationController : Controller
    {
        private IStoreLocationBL _storeLocationBL;
        private IManagerBL _managerBL;
        public StoreLocationController(IStoreLocationBL storeLocationBL, IManagerBL managerBL)
        {
            _storeLocationBL = storeLocationBL;
            _managerBL = managerBL;
        }
        // GET: StoreLocationController
        public ActionResult Index(long id)
        {
            ViewBag.DogManager = _managerBL.FindManager(id);
            List<StoreLocationVM> storeLocations = _managerBL.GetManagerStores(id)
                        .Select(storeLoc => new StoreLocationVM(storeLoc)).ToList();
            /*foreach(StoreLocationVM sVM in storeLocations)
            {
                sVM.CurrentManager = id;
            }*/
            return View(storeLocations);
        }
        public ActionResult CustomerIndex()
        {
            List<StoreLocationVM> stores = _storeLocationBL.GetAllStoreLocations()
                                            .Select(storeLoc => new StoreLocationVM(storeLoc)).ToList();
            return View(stores);
        }
        // GET: StoreLocationController/Details/5
        public ActionResult Details(long id)
        {
            return View();
        }

        // GET: StoreLocationController/Create
        public ActionResult Create(long id)
        {
            StoreLocationVM store = new StoreLocationVM();
            store.CurrentManager = id;
            ViewBag.DogManager = _managerBL.FindManager(id);

            return View(store);
        }

        // POST: StoreLocationController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(StoreLocationVM storeLocationVM)
        {
            try
            {
                long test = storeLocationVM.CurrentManager;
                int i = 1;
               // if (ModelState.IsValid)
               // {
                    _storeLocationBL.AddStoreLocation(new DSModels.StoreLocation
                    {
                        Address = storeLocationVM.Address,
                        Location = storeLocationVM.Location
                    }, _managerBL.FindManager(storeLocationVM.CurrentManager)
                        );

                    return RedirectToAction(nameof(Index), new { id = storeLocationVM.CurrentManager});
               // }
                //else return View();
            }
            catch
            {
                return View();
            }
        }

        // GET: StoreLocationController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: StoreLocationController/Edit/5
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

        // GET: StoreLocationController/Delete/5
        public ActionResult Delete(int id, long num)
        {
            
            _storeLocationBL.RemoveStore(id);
            return RedirectToAction(nameof(Index), new { id = num});
        }

        // POST: StoreLocationController/Delete/5
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
