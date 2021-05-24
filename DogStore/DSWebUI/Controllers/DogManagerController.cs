using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DSBL;
using DSWebUI.Models;
namespace DSWebUI.Controllers
{
    public class DogManagerController : Controller
    {
        private IManagerBL _managerBL;
        public DogManagerController(IManagerBL managerBL)
        {
            _managerBL = managerBL;
        }
        // GET: DogManagerController
        public ActionResult Index()
        {
            return View(_managerBL.GetAllManagers()
                        .Select(manager => new DogManagerVM(manager)).ToList());
                    
        }

        // GET: DogManagerController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: DogManagerController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DogManagerController/Create
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

        // GET: DogManagerController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: DogManagerController/Edit/5
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

        // GET: DogManagerController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: DogManagerController/Delete/5
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
