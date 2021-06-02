using DSBL;
using DSModels;
using DSWebUI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DSWebUI.Controllers
{
    public class OrderHistoryController : Controller
    {
        private IBuyerBL _buyerBL;
        private IStoreLocationBL _storeLocationBL;
        public IOrderBL _orderBL;
        public OrderHistoryController(IBuyerBL buyerBL, IStoreLocationBL storeLocationBL, IOrderBL orderBL)
        {
            _buyerBL = buyerBL;
            _storeLocationBL = storeLocationBL;
            _orderBL = orderBL;
        }
        public ActionResult StoOrdIndex(int id, int option)
        {
            StoreLocation store = _storeLocationBL.GetStore(id);
            List<DogOrder> dogOrders = _orderBL.FindStoreOrders(store.Address, store.Location, option);
            List<BuyerHistoryVM> buyerHistories = new List<BuyerHistoryVM>();

            foreach (DogOrder dO in dogOrders)
            {
                BuyerHistoryVM buyerHistory = new BuyerHistoryVM();
                buyerHistory.OrderId = dO.Id;
                if (_buyerBL.FindUser(dO.BuyerId) != null)
                {
                    DogBuyer dogBuyer = _buyerBL.FindUser(dO.BuyerId);
                    buyerHistory.BuyerName = dogBuyer.Name;
                    buyerHistory.BuyerNumber = id;
                    
                }
                else
                {
                    buyerHistory.BuyerName = "Not Found";
                    buyerHistory.BuyerNumber = 0;
                }
                buyerHistory.StoreName = store.Location;
                buyerHistory.Address = store.Address;
                buyerHistory.Total = Math.Round(dO.Total,2);
                buyerHistory.OrderDate = dO.OrderDate;
                buyerHistories.Add(buyerHistory);
            }
            return View(buyerHistories);
        }
        // GET: OrderHistoryController
        public ActionResult Index(long id, int option)
        {
            List<DogOrder> dogOrders = _orderBL.FindUserOrders(id, option);
            List<BuyerHistoryVM> buyerHistories = new List<BuyerHistoryVM>();
            DogBuyer dogBuyer = _buyerBL.FindUser(id);

            foreach( DogOrder dO in dogOrders)
            {
                BuyerHistoryVM buyerHistory = new BuyerHistoryVM();
                buyerHistory.OrderId = dO.Id;
                if (_storeLocationBL.GetStore(dO.StoreId) != null)
                {
                    StoreLocation storeLocation = _storeLocationBL.GetStore(dO.StoreId);
                    buyerHistory.StoreName = storeLocation.Location;
                    buyerHistory.Address = storeLocation.Address;
                }
                else
                {
                    buyerHistory.StoreName = "Store Deleted";
                    buyerHistory.Address = "Store Deleted";
                }
                buyerHistory.BuyerName = dogBuyer.Name;
                buyerHistory.BuyerNumber = id;
                buyerHistory.Total = Math.Round(dO.Total,2);
                buyerHistory.OrderDate = dO.OrderDate;
                buyerHistories.Add(buyerHistory);
            }
            return View(buyerHistories);
        }

        // GET: OrderHistoryController/Details/5
        public ActionResult Details(int id)
        {
            List<OrderItem> orderItems = _orderBL.GetOrderItems(id);
            List<OrderItemVM> orderItemVMs = new List<OrderItemVM>();
            foreach(OrderItem orderItem in orderItems)
            {
                
                if (_storeLocationBL.GetDog(orderItem.DogId) != null)
                {
                    OrderItemVM orderItemVM = new OrderItemVM();
                    Dog dog = _storeLocationBL.GetDog(orderItem.DogId);
                    orderItemVM.Breed = dog.Breed;
                    orderItemVM.Gender = dog.Gender;
                    orderItemVM.Price = dog.Price;
                    orderItemVM.Quantity = orderItem.Quantity;
                    orderItemVMs.Add(orderItemVM);
                }
                else { }
                

            }
            return View(orderItemVMs);
        }

        // GET: OrderHistoryController/Create
        public ActionResult Create(long id)
        {
            BuyerHistoryVM buyerHistoryVM = new BuyerHistoryVM();
            buyerHistoryVM.BuyerNumber = id;
            return View(buyerHistoryVM);
        }
        public ActionResult StoHisCreate(int id)
        {
            BuyerHistoryVM buyerHistoryVM = new BuyerHistoryVM();
            buyerHistoryVM.StoreId = id;
            return View(buyerHistoryVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult StoHisCreate(BuyerHistoryVM buyerHistoryVM)
        {
            try
            {
                return RedirectToAction(nameof(StoOrdIndex), new { id = buyerHistoryVM.StoreId, option = buyerHistoryVM.OrderOption });
            }
            catch
            {
                return View();
            }
        }
        // POST: OrderHistoryController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BuyerHistoryVM buyerHistoryVM)
        {
            try
            {
                return RedirectToAction(nameof(Index),new { id = buyerHistoryVM.BuyerNumber, option = buyerHistoryVM.OrderOption });
            }
            catch
            {
                return View();
            }
        }

        // GET: OrderHistoryController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: OrderHistoryController/Edit/5
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

        // GET: OrderHistoryController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: OrderHistoryController/Delete/5
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
