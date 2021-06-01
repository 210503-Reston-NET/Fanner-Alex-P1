using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DSBL;
using DSWebUI.Models;
using DSModels;
using Serilog;

namespace DSWebUI.Controllers
{
    public class DogBuyerController : Controller
    {
        private IBuyerBL _buyerBL;
        public DogBuyerController(IBuyerBL buyerBL)
        {
            _buyerBL = buyerBL;
        }
        // GET: DogBuyerController
        public ActionResult Index()
        {
            return View(_buyerBL.GetAllBuyers()
                        .Select(buyer => new DogBuyerVM(buyer)).ToList());
        }

        // GET: DogBuyerController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: DogBuyerController/Create
        public ActionResult Create()
        {
            Log.Information("Going to Buyer Create Menu");
            return View();
            
        }

        // POST: DogBuyerController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DogBuyerVM dogBuyerVM)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _buyerBL.AddBuyer(new DSModels.DogBuyer
                    {
                        Name = dogBuyerVM.Name,
                        Address = dogBuyerVM.Address,
                        PhoneNumber = dogBuyerVM.PhoneNumber
                    }
                        );
                    Log.Information("Buyer with number = " + dogBuyerVM.PhoneNumber.ToString()+" added");
                    return RedirectToAction(nameof(Index));
                }
                else return View();
            }
            catch
            {
                return View();
            }
        }

        // GET: DogBuyerController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: DogBuyerController/Edit/5
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

        // GET: DogBuyerController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: DogBuyerController/Delete/5
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
