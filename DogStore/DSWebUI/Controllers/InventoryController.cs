using DSBL;
using DSModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DSWebUI.Models;
namespace DSWebUI.Controllers
{
    public class InventoryController : Controller
    {
        private IStoreLocationBL _storeLocationBL;
        public InventoryController(IStoreLocationBL storeLocationBL)
        {
            _storeLocationBL = storeLocationBL;
        }
        // GET: InventoryController
        public ActionResult Index(int id)
        {
            StoreLocation sL = _storeLocationBL.GetStore(id);
            ViewBag.StoreLocation = _storeLocationBL.GetStore(id);
            List<InventoryVM> items = _storeLocationBL.GetStoreInventory(sL.Address, sL.Location)
                        .Select(storeLoc => new InventoryVM(storeLoc)).ToList();
            //TODO
            return View();
        }

        // GET: InventoryController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: InventoryController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: InventoryController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
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

        // GET: InventoryController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: InventoryController/Edit/5
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

        // GET: InventoryController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: InventoryController/Delete/5
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
