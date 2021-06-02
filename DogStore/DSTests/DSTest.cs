
using System;
using Xunit;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using DSDL;
using Model = DSModels;

using System.Linq;
using DSModels;
using DSBL;

namespace DSTests
{
    public class DSTest
    {
        private readonly DbContextOptions<FannerDogsDBContext> options;
        public DSTest()
        {
            options = new DbContextOptionsBuilder<FannerDogsDBContext>().UseSqlite("Filename=Test.db").Options;
            Seed();
        }
        [Fact]
        /// <summary>
        /// Checks AddManager and FindManager for functionality
        /// </summary>
        public void AddManagerAddsManager()
        {
            using (var context = new FannerDogsDBContext(options))
            {
                IRepo _repoDS = new Repo(context);
                Model.DogManager dogManager= new Model.DogManager(1234567890,"Test, TX","Texas Toaster");
                _repoDS.AddManager
                (
                    dogManager
                );
                Model.DogManager dogManagerReturned = _repoDS.FindManager(1234567890);
                Assert.Equal(dogManagerReturned.PhoneNumber, dogManager.PhoneNumber);
            }
        }
        [Fact]
        /// <summary>
        /// Checks AddStoreLocation and FindStore
        /// </summary>
        public void AddStoreLocationAddsStore()
        {
            using (var context = new FannerDogsDBContext(options))
            {
                IRepo _repoDS = new Repo(context);
                Model.DogManager dogManager= new Model.DogManager(1234567890,"Test, TX","Texas Toaster");
                _repoDS.AddManager
                (
                    dogManager
                );
                _repoDS.AddStoreLocation(
                    new Model.StoreLocation("Test, TX", "Test Dogs"),
                    dogManager
                );
                Model.StoreLocation store = _repoDS.FindStore("Test, TX", "Test Dogs");
                bool storeThere = (store != null);
                bool expected = true;
                Assert.Equal(storeThere, expected);
            }
        }
        [Fact]
        /// <summary>
        /// Checks AddBuyer and FindBuyer
        /// </summary>
        public void AddBuyerAddsBuyer()
        {
            using (var context = new FannerDogsDBContext(options))
            {
                IRepo _repoDS = new Repo(context);
                Model.DogBuyer dogBuyer= new Model.DogBuyer("Texas Toaster","Test, TX",1234567890);
                _repoDS.AddBuyer
                (
                    dogBuyer
                );
                Model.DogBuyer dogBuyerReturned = _repoDS.FindBuyer(1234567890);
                Assert.Equal(dogBuyerReturned.PhoneNumber, dogBuyer.PhoneNumber);
            }
        }
        [Fact]
        /// <summary>
        /// Checks GetAllDogManagers
        /// </summary>
        public void GetAllManagersGetsManagers()
        {
            using (var context = new FannerDogsDBContext(options))
            {
                IRepo _repoDS = new Repo(context);
                Model.DogManager dogManager= new Model.DogManager(9638527410,"Wired, Wyoming","Ama Test");
                _repoDS.AddManager
                (
                    dogManager
                );
                dogManager= new Model.DogManager(1234567890,"Test, TX","Texas Toaster");
                _repoDS.AddManager
                (
                    dogManager
                );
                List<Model.DogManager> dogManagers = _repoDS.GetAllDogManagers();
                int expected = 2;
                Assert.Equal(dogManagers.Count, expected);
            }
        }
        [Fact]
        /// <summary>
        /// Checks GetAllBuyers
        /// </summary>
        public void GetAllBuyersGetsBuyers()
        {
            using (var context = new FannerDogsDBContext(options))
            {
                IRepo _repoDS = new Repo(context);
                Model.DogBuyer dogBuyer = new Model.DogBuyer("Ama Test","Wired, Wyoming",9638527410);
                _repoDS.AddBuyer
                (
                    dogBuyer
                );
                dogBuyer= new Model.DogBuyer("Texas Toaster","Test, TX",1234567890);
                _repoDS.AddBuyer
                (
                    dogBuyer
                );
                List<Model.DogBuyer> dogBuyers = _repoDS.GetAllBuyers();
                int expected = 2;
                Assert.Equal(dogBuyers.Count, expected);
            }
        }

        [Fact]
        /// <summary>
        /// Checks AddItem and FindItem
        /// </summary>
        public void AddItemShouldBeFound(){
            using (var context = new FannerDogsDBContext(options))
            {
                IRepo _repoDS = new Repo(context);
                Model.DogManager dogManager= new Model.DogManager(1234567890,"Test, TX","Texas Toaster");
                _repoDS.AddManager
                (
                    dogManager
                );
                Model.StoreLocation storeLocation = new Model.StoreLocation("Test, TX", "Test Dogs");
                _repoDS.AddStoreLocation(
                    storeLocation,
                    dogManager
                );
                Model.Dog dog = new Model.Dog("Special Breed",'f',1000);
                _repoDS.AddItem(
                    storeLocation,
                    dog,
                    5
                );
                Model.Item item = _repoDS.FindItem(
                    storeLocation,
                    dog,
                    5
                );
                bool itemThere = (item != null);
                bool expected = true;
                Assert.Equal(itemThere, expected);
            }
        }

        [Fact]
        /// <summary>
        /// Makes sure store inventory is being updated
        /// </summary>
        public void AddItemShouldBeInStoreInventory(){
            using (var context = new FannerDogsDBContext(options))
            {
                IRepo _repoDS = new Repo(context);
                Model.DogManager dogManager= new Model.DogManager(1234567890,"Test, TX","Texas Toaster");
                _repoDS.AddManager
                (
                    dogManager
                );
                Model.StoreLocation storeLocation = new Model.StoreLocation("Test, TX", "Test Dogs");
                _repoDS.AddStoreLocation(
                    storeLocation,
                    dogManager
                );
                Model.Dog dog = new Model.Dog("Special Breed",'f',1000);
                _repoDS.AddItem(
                    storeLocation,
                    dog,
                    5
                );
                List<Model.Item> items = _repoDS.GetStoreInventory(
                    storeLocation.Address,
                    storeLocation.Location
                );
                int expected = 1;
                Assert.Equal(items.Count(), expected);
            }

        }
        [Fact]
        /// <summary>
        /// Checks FindItem to make sure it returns null correctly if Item quantity is too high
        /// </summary>
        public void WrongItemShouldNotBeFound(){
            using (var context = new FannerDogsDBContext(options))
            {
                IRepo _repoDS = new Repo(context);
                Model.DogManager dogManager= new Model.DogManager(1234567890,"Test, TX","Texas Toaster");
                _repoDS.AddManager
                (
                    dogManager
                );
                Model.StoreLocation storeLocation = new Model.StoreLocation("Test, TX", "Test Dogs");
                _repoDS.AddStoreLocation(
                    storeLocation,
                    dogManager
                );
                Model.Dog dog = new Model.Dog("Special Breed",'f',1000);
                _repoDS.AddItem(
                    storeLocation,
                    dog,
                    5
                );
                Model.Item item = _repoDS.FindItem(
                    storeLocation,
                    dog,
                    20
                );
                bool itemNotThere = (item == null);
                bool expected = true;
                Assert.Equal(itemNotThere, expected);
            }
        }
        [Fact]
        /// <summary>
        /// Checks FindBuyer to make sure it returns null appropriately
        /// </summary>
        public void WrongBuyerShouldNotBeFound()
        {
            using (var context = new FannerDogsDBContext(options))
            {
                IRepo _repoDS = new Repo(context);
                Model.DogBuyer dogBuyer= new Model.DogBuyer("Texas Toaster","Test, TX",1234567890);
                _repoDS.AddBuyer
                (
                    dogBuyer
                );
                Model.DogBuyer dogBuyerReturned = _repoDS.FindBuyer(1235467890);
                bool buyerNotThere = (dogBuyerReturned == null);
                bool expected = true;
                Assert.Equal(buyerNotThere, expected);
            }
        }
        [Fact]
        /// <summary>
        /// Checks FindManager to make sure it returns null appropriately
        /// </summary>
        public void WrongManagerShouldNotBeFound()
        {
            using (var context = new FannerDogsDBContext(options))
            {
                IRepo _repoDS = new Repo(context);
                Model.DogManager dogManager= new Model.DogManager(1234567890,"Test, TX","Texas Toaster");
                _repoDS.AddManager
                (
                    dogManager
                );
                Model.DogManager dogManagerReturned = _repoDS.FindManager(3214567890);
                bool managerNotThere = (dogManagerReturned == null);
                bool expected = true;
                Assert.Equal(managerNotThere, expected);
            }
        }
        /// <summary>
        /// Make sure new method GetManagerStores works by adding one and seeing if comes back correctly 
        /// </summary>
        [Fact]
        public void GetManagerStoresShouldWork()
        {
            using (var context = new FannerDogsDBContext(options))
            {
                IRepo _repoDS = new Repo(context);
                DogManager dogMananger = new DogManager(1234567890, "Test, TX", "Texas Toaster");
                _repoDS.AddManager(dogMananger);
                _repoDS.AddStoreLocation(new StoreLocation("Arbille, FL", "abill dogs"), dogMananger);
                int GetManStorCount = _repoDS.GetManagerStores(1234567890).Count;
                int expected = 1;
                Assert.Equal(expected, GetManStorCount);
            }

        }
        /// <summary>
        /// Making sure new add and find dog methods work out
        /// </summary>
        [Fact]
        public void AddDogShouldBeFound()
        {
            using (var context = new FannerDogsDBContext(options))
            {
                IRepo _repoDS = new Repo(context);
                _repoDS.AddDog(new Dog("Shiba Inu", 'f', 300.50));
                Dog dog = _repoDS.FindDog("Shiba Inu", 'f');
                string expected = "Shiba Inu";
                string actual = dog.Breed;
                Assert.Equal(expected, actual);
            }
        }
        /// <summary>
        /// Testing new dec inv method
        /// </summary>
        [Fact]
        public void DecInvShouldDecrement()
        {
            using (var context = new FannerDogsDBContext(options))
            {
                IRepo _repoDS = new Repo(context);
                Model.DogManager dogManager = new Model.DogManager(1234567890, "Test, TX", "Texas Toaster");
                _repoDS.AddManager
                (
                    dogManager
                );
                Model.StoreLocation storeLocation = new Model.StoreLocation("Test, TX", "Test Dogs");
                _repoDS.AddStoreLocation(
                    storeLocation,
                    dogManager
                );
                Model.Dog dog = new Model.Dog("Special Breed", 'f', 1000);
                _repoDS.AddItem(
                    storeLocation,
                    dog,
                    5
                );
                _repoDS.DecInv(_repoDS.FindStore("Test, TX", "Test Dogs").Id, _repoDS.FindDog("Special Breed", 'f').Id, 2);
                Item inv = _repoDS.GetStoreInventory("Test, TX", "Test Dogs")[0];
                int expected = 3;
                int actual = inv.Quantity;
                Assert.Equal(expected, actual);
            }
        }
        /// <summary>
        /// Testing the add dog and get dogs methods
        /// </summary>
        [Fact]
        public void AddDogShouldBeGotten()
        {
            using (var context = new FannerDogsDBContext(options))
            {
                IRepo _repoDS = new Repo(context);
                _repoDS.AddDog(new Dog("Special Breed", 'f', 1000));
                int expected = 1;
                int actual = _repoDS.GetDogs().Count;
                Assert.Equal(expected, actual);
            }
        }
        /// <summary>
        /// Making sure adding the same dog returns null
        /// </summary>
        [Fact]
        public void DoubleAddedDogShouldBeNull()
        {
            using (var context = new FannerDogsDBContext(options))
            {
                IRepo _repoDS = new Repo(context);
                _repoDS.AddDog(new Dog("Special Breed", 'f', 1000));
                Assert.Null(_repoDS.AddDog(new Dog("Special Breed", 'f', 1000)));
            }
        }
        /// <summary>
        /// Makes sure new GetOrder method works
        /// </summary>
        [Fact]
        public void GetOrderByIdGetsOrder()
        {
            using (var context = new FannerDogsDBContext(options))
            {
                IRepo _repoDS = new Repo(context);
                Model.DogManager dogManager = new Model.DogManager(1234567890, "Test, TX", "Texas Toaster");
                _repoDS.AddManager
                (
                    dogManager
                );
                Model.StoreLocation storeLocation = new Model.StoreLocation("Test, TX", "Test Dogs");
                _repoDS.AddStoreLocation(
                    storeLocation,
                    dogManager
                );
                storeLocation = _repoDS.FindStore("Test, TX", "Test Dogs");
                DogBuyer dogBuyer = new Model.DogBuyer("Ama Test", "Wired, Wyoming", 9638527410);
                _repoDS.AddBuyer(dogBuyer);
                DogOrder addingOrder = new DogOrder(dogBuyer, 0, storeLocation);
                addingOrder.OrderDate = DateTime.Now;
                _repoDS.AddOrder(addingOrder);
                DogOrder dogOrder = _repoDS.FindUserOrders(dogBuyer.PhoneNumber,1)[0];
                DogOrder order = _repoDS.GetOrder(dogOrder.Id);
                long expected = dogBuyer.PhoneNumber;
                long actual = order.BuyerId;
                Assert.Equal(expected, actual);

            }
        }
        /// <summary>
        /// Makes sure AddOrderItem throws null if requested too much of an item
        /// </summary>
        [Fact]
        public void AddOrderItemNullIfTooMuch()
        {
            using (var context = new FannerDogsDBContext(options))
            {
                IOrderBL _orderBL = new OrderBL(context);
                Assert.Null(_orderBL.AddOrderItem(new OrderItem() { Quantity = 5},4,1));
            }
        }
        /// <summary>
        /// Makes sure AddOrderItem throws null if requested 0 much of an item
        /// </summary>
        [Fact]
        public void AddOrderItemNullIfNone()
        {
            using (var context = new FannerDogsDBContext(options))
            {
                IOrderBL _orderBL = new OrderBL(context);
                Assert.Null(_orderBL.AddOrderItem(new OrderItem() { Quantity = 5 }, 4, 1));
            }
        }
        private void Seed(){
            using(var context = new FannerDogsDBContext(options)){
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
                
            }
        }
    }
}
