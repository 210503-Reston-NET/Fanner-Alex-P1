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
        public ActionResult Index(int id)
        {
            ViewBag.OrderId = id;
            List<OrderItem> orderItems = _orderBL.GetOrderItems(id);
            List<OrderItemVM> orderItemVMs = new List<OrderItemVM>();
            foreach(OrderItem o in orderItems)
            {
                OrderItemVM orderItemVM = new OrderItemVM();
                Dog dog = _storeLocationBL.GetDog(o.DogId);
                orderItemVM.DogId = dog.Id;
                orderItemVM.Breed = dog.Breed;
                orderItemVM.Gender = dog.Gender;
                orderItemVM.Quantity = o.Quantity;
                orderItemVM.Price = dog.Price;
                orderItemVMs.Add(orderItemVM);
            }
            return View(orderItemVMs);
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
        public ActionResult CompleteOrder(int id)
        {
            DogOrder dogOrder = _orderBL.GetOrder(id);
            dogOrder.OrderDate = DateTime.Now;
            _orderBL.UpdateOrder(dogOrder);
            return View("../Home/Index");
        }
        public ActionResult CreateOrderItem(int id)
        {
            DogOrder order = _orderBL.GetOrder(id);
            StoreLocation storeLocation = _storeLocationBL.GetStore(order.StoreId);
            List<Item> items = _storeLocationBL.GetStoreInventory(storeLocation.Address, storeLocation.Location);
            ViewBag.OrderId = id;
            List<string> dogStrings = new List<string>();
            foreach(Item i in items)
            {
                Dog dog = _storeLocationBL.GetDog(i.DogId);
                string dogString = dog.Breed + ", " + dog.Gender.ToString() + ", " + dog.Price.ToString() + " USD, Count: " + i.Quantity.ToString();
                dogStrings.Add(dogString);
            }
            OrderItemVM orderItemVM = new OrderItemVM();
            orderItemVM.OrderId = id;
            orderItemVM.DogStringList = dogStrings;
            return View(orderItemVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateOrderItem(OrderItemVM orderItemVM) {
            try
            {
                string[] OrderParts = orderItemVM.DogString.Split(new string[] { ", " }, StringSplitOptions.None);
                orderItemVM.Breed = OrderParts[0];
                orderItemVM.Gender = OrderParts[1].ToCharArray()[0];
                orderItemVM.Price = double.Parse(OrderParts[2].Remove(OrderParts[2].Length - 3));
                int maxQuant = int.Parse(OrderParts[3].Split(new string[] { ": " }, StringSplitOptions.None)[1]);
                Dog dog = _storeLocationBL.FindDog(orderItemVM.Breed, orderItemVM.Gender);
                DogOrder dogOrder = _orderBL.GetOrder(orderItemVM.OrderId);
                int storeId = dogOrder.StoreId;
                OrderItem orderItem = new OrderItem();
                orderItem.OrderId = dogOrder.Id;
                orderItem.DogId = dog.Id;
                orderItem.Quantity = orderItemVM.Quantity;
                if(_orderBL.AddOrderItem(orderItem, maxQuant, storeId) == null) return RedirectToAction(nameof(Index), new { id = orderItemVM.OrderId });
                dogOrder.Total += orderItemVM.Price*orderItemVM.Quantity;
                _orderBL.UpdateOrder(dogOrder);
                return RedirectToAction(nameof(Index), new { id = orderItemVM.OrderId });
            }
            catch
            {
                return RedirectToAction(nameof(Index), new { id = orderItemVM.OrderId });
            }

        }
        // POST: DogOrderController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DogOrderVM dogOrderVM)
        {
            try
            {
                DogBuyer dogBuyer = _buyerBL.FindUser(dogOrderVM.BuyerId);
                StoreLocation storeLoc = _storeLocationBL.GetStore(dogOrderVM.StoreId);
                
                DogOrder dogOrder = new DogOrder(dogBuyer, 0, storeLoc);
                dogOrder = _orderBL.AddOrder(dogOrder);

                return RedirectToAction(nameof(Index), new { id = dogOrder.Id});
            }
            catch
            {
                return View(dogOrderVM.StoreId);
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
