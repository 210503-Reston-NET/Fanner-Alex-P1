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
            List<Item> items = _storeLocationBL.GetStoreInventory(sL.Address, sL.Location);
            //TODO
            List<InventoryVM> invs = new List<InventoryVM>();
            Dog itemDog;
            InventoryVM inv;
            foreach(Item item in items)
            {
                inv = new InventoryVM(item);
                itemDog = _storeLocationBL.GetDog(item.DogId);
                inv.Breed = itemDog.Breed;
                inv.Gender = itemDog.Gender;
                inv.Price = itemDog.Price;
                invs.Add(inv);
            }
            return View(invs);
        }
       
        // GET: InventoryController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: InventoryController/Create
        public ActionResult Create(int id)
        {
            List<Dog> dogs = _storeLocationBL.GetDogs();
            List<string> dogStrings = new List<string>();
            foreach(Dog dog in dogs)
            {

                string dogString = dog.Breed + ", " + dog.Gender.ToString() + ", " + dog.Price.ToString() + " USD";
                dogStrings.Add(dogString);
            }
            InventoryVM invent = new InventoryVM();
            invent.StoreLocationId = id;
            invent.DogStringList = dogStrings;
            return View(invent);
        }

        // POST: InventoryController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(InventoryVM inventoryVM)
        {
            try
            {
                string[] OrderParts = inventoryVM.DogString.Split(new string[] { ", " }, StringSplitOptions.None);
                inventoryVM.Breed = OrderParts[0];
                inventoryVM.Gender = OrderParts[1].ToCharArray()[0];
                inventoryVM.Price = double.Parse(OrderParts[2].Remove(OrderParts[2].Length - 3));
                StoreLocation storeLocation = _storeLocationBL.GetStore(inventoryVM.StoreLocationId);
                // if (ModelState.IsValid)
                // {
                Dog dog = new Dog(inventoryVM.Breed, inventoryVM.Gender, inventoryVM.Price);
                _storeLocationBL.AddItem(storeLocation, dog, inventoryVM.Quantity);

                return RedirectToAction(nameof(Index), new { id = inventoryVM.StoreLocationId });
                // }
                //else return View();
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
