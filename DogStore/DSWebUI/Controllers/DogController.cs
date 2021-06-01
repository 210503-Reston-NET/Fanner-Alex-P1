using DSModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DSBL;
using DSWebUI.Models;
using Serilog;

namespace DSWebUI.Controllers
{
    public class DogController : Controller
    {
        IStoreLocationBL _storeLocationBL;
        public DogController(IStoreLocationBL storeLocationBL)
        {
            _storeLocationBL = storeLocationBL;
        }
        // GET: DogController
        public ActionResult Index()
        {
            List<Dog> dogs = _storeLocationBL.GetDogs();
            List<DogVM> dogVMs = new List<DogVM>();
            foreach(Dog d in dogs)
            {
                DogVM dogVM = new DogVM();
                dogVM.Id = d.Id;
                dogVM.Breed = d.Breed;
                dogVM.Gender = d.Gender;
                dogVM.Price = d.Price;
                dogVMs.Add(dogVM);
            }
            return View(dogVMs);
        }

        // GET: DogController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: DogController/Create
        public ActionResult Create()
        {
            DogVM dogVM = new DogVM();
            Log.Information("Sending user to New Dog Creation");
            return View(dogVM);
        }

        // POST: DogController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DogVM dogVM)
        {
            try
            {
                Dog dog = new Dog();
                dog.Breed = dogVM.Breed;
                dog.Gender = dogVM.Gender;
                dog.Price = dogVM.Price;
                _storeLocationBL.AddDog(dog);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: DogController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: DogController/Edit/5
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

        // GET: DogController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: DogController/Delete/5
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
