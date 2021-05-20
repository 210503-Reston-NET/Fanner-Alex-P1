using System.Diagnostics;

using System.Linq;
using System.Collections.Generic;
using Model = DSModels;
using Entity = DSDL.Entities;
using DSModels;
using System.IO;
using System.Text.Json;
using System;
using Microsoft.EntityFrameworkCore;
using Serilog;
namespace DSDL
{
    /// <summary>
    /// Repository class to store data in SQL database
    /// </summary>
    public class Repo : IRepo
    {
        private List<Model.StoreLocation> _stores;
        
        private Entity.FannerDogsDBContext _context;
        public Repo(Entity.FannerDogsDBContext context){
            _context = context;
            Log.Debug("Created an instance of the repository");
        }
        /// <summary>
        /// Method to add store location to the file. Adds a store to a file and returns
        /// the added store.
        /// </summary>
        /// <param name="store">StoreLocation to add to memory</param>
        // <returns>Return added StoreLocation</returns>
        public Model.StoreLocation AddStoreLocation(Model.StoreLocation store, Model.DogManager dogManager)
        {
            try{
                Entity.DogStore dogStore = new Entity.DogStore();
                dogStore.StoreName = store.Location;
                dogStore.StoreAddress = store.Address;
                _context.DogStores.Add(
                    dogStore
                );
                Entity.ManagesStore managesStore = new Entity.ManagesStore();
                
                _context.SaveChanges();
                Entity.DogStore dS = (
                                        from DogStore in _context.DogStores where 
                                        DogStore.StoreAddress == dogStore.StoreAddress && DogStore.StoreName == dogStore.StoreName
                                        select DogStore
                                        ).Single();
                managesStore.ManagerId = dogManager.PhoneNumber;
                managesStore.StoreId = dS.Id;
                _context.ManagesStores.Add(managesStore);
                _context.SaveChanges();
            }
            catch(Exception ex){
                Log.Error(ex.Message+ "error encountered in AddStoreLocation, this shouldn't happen");
            }
            return store;
        }

        /// <summary>
        /// Method that returns all the stores in memory.
        /// </summary>
        /// <returns>List of StoreLocation stored in the JSON</returns>
        public List<Model.StoreLocation> GetAllStoreLocations()
        {
            List<Model.StoreLocation> storeList = new List<Model.StoreLocation>();
            List<Entity.DogStore> dogStoreList = (from DogStore in _context.DogStores select DogStore).ToList();
            foreach(Entity.DogStore dS in dogStoreList){
                storeList.Add(new StoreLocation(dS.Id, dS.StoreAddress,dS.StoreName));
            }
            return storeList;
        }
        /// <summary>
        /// Gets a store from memory and returns the Inventory as a List of Items.
        /// </summary>
        /// <param name="address"> Address of store you're looking for</param>
        /// <param name="location"> Name of store you're looking for</param>
        /// <returns>List of items responding to the store's inventory.</returns>
        public List<Model.Item> GetStoreInventory(string address, string location)
        {
            StoreLocation sL = new StoreLocation(address, location);
            try{
                Entity.DogStore dS = (
                                        from DogStore in _context.DogStores where 
                                        DogStore.StoreAddress == address && DogStore.StoreName == location
                                        select DogStore
                                        ).Single();
                List<Entity.Inventory> iList = (
                                        from Inventory in _context.Inventories where
                                        Inventory.StoreId == dS.Id
                                        select Inventory
                                        ).ToList();
                List<Model.Item> itemList = new List<Model.Item>();
                foreach(Entity.Inventory i in iList){
                    Entity.Dog dog = (
                                        from Dog in _context.Dogs where
                                        Dog.ItemId == i.DogId
                                        select Dog
                    ).Single();
                    Log.Information("Here's the item I'm about to create");
                    Log.Information(dog.Breed);
                    Log.Information(dog.Gender.ToCharArray()[0].ToString());
                    Log.Information(dog.Price.ToString());
                    itemList.Add(new Model.Inventory(new Model.Dog(dog.Breed,dog.Gender.ToCharArray()[0], dog.Price),i.Quantity.Value));
                }
                return itemList;
            } catch(Exception e){
                Log.Error(e.Message+ " some issue with finding this inventory");
                return new List<Model.Item>();
            }
        }
        /// <summary>
        /// Adds an item to a stores inventory, creates dog if not found then adds it to the inventory
        /// </summary>
        /// <param name="store">Store to add inventory to</param>
        /// <param name="dog">Dog to add</param>
        /// <param name="quant">Quantity of Dog to add</param>
        /// <returns>Added item</returns>
        public Model.Item AddItem(StoreLocation store, Dog dog, int quant)
        {
            Item newItem = new Model.Inventory(dog, quant);
            try{
                Entity.Dog searchDog = (
                                        from Dog in _context.Dogs where 
                                        Dog.Breed == dog.Breed && Dog.Gender == dog.Gender.ToString()
                                        select Dog
                                        ).Single();
                Entity.DogStore dS = (
                                        from DogStore in _context.DogStores where 
                                        DogStore.StoreAddress == store.Address && DogStore.StoreName == store.Location
                                        select DogStore
                                        ).Single();
                
                try{

                    Entity.Inventory inv = (
                                        from Inventory in _context.Inventories where
                                        Inventory.StoreId == dS.Id && Inventory.DogId == searchDog.ItemId
                                        select Inventory
                                        ).Single();
                    inv.Quantity += quant;
                    _context.SaveChanges();
                    Log.Information("Item found, quanty incremented: " + quant.ToString());
                    return newItem;
                }
                catch(Exception e){
                    Log.Information(e.Message + "dog found but not inventory, adding dog to store's inventory");
                    Entity.Inventory inventory = new Entity.Inventory();
                    inventory.DogId = searchDog.ItemId;
                    inventory.Quantity = quant;
                    inventory.StoreId = dS.Id;
                    _context.Inventories.Add(inventory);
                    _context.SaveChanges();
                    return newItem;
                }
            }
            catch (Exception e){
                Log.Information(e.Message + "Dog not found, creating new dog");
                Entity.Dog newDog = new Entity.Dog();
                newDog.ItemId = new Random().Next();
                newDog.Breed = dog.Breed;
                newDog.Gender = dog.Gender.ToString();
                newDog.Price = dog.Price;
                _context.Dogs.Add(newDog);
                _context.SaveChanges();
                Entity.Dog searchDog = newDog;
                Entity.DogStore dS = (
                                        from DogStore in _context.DogStores where 
                                        DogStore.StoreAddress == store.Address && DogStore.StoreName == store.Location
                                        select DogStore
                                        ).Single();
                Entity.Inventory inventory = new Entity.Inventory();
                    inventory.DogId = searchDog.ItemId;
                    inventory.Quantity = quant;
                    inventory.StoreId = dS.Id;
                    _context.Inventories.Add(inventory);
                    _context.SaveChanges();
                    return newItem;
            }
        }

        /// <summary>
        /// Finds and returns the result of a LINQ query which matches on an 
        /// address and location of a store.
        /// </summary>
        /// <param name="address"> Address of the store you're looking for.</param>
        /// <param name="location"> Location name of the store you're looking for.</param>
        /// <returns></returns>
        public Model.StoreLocation FindStore(string address, string location){
            //StoreLocation store = new StoreLocation(address, location);
            return GetAllStoreLocations().First(stor => stor.Address == address && stor.Location == location);
            //from DogStore in _context.DogStores where 
        }
        /// <summary>
        /// Finds a store you're looking for and removes it NOT IMPLEMENTED
        /// </summary>
        /// <param name="address"> Address of the store you want to remove.</param>
        /// <param name="location"> Name of the store you want to remove.</param>
        /// <returns> Store which was removed from memory.</returns>
        public Model.StoreLocation RemoveStore(string address, string location){
            List<StoreLocation> storesFromFile = GetAllStoreLocations();
            StoreLocation store = FindStore(address, location);
            _stores.Remove(store);
            return store;
        }
        /// <summary>
        /// Method which searches for a quantity of a dog at a given store,
        /// throws an exception and returns null if item request is invalid.
        /// </summary>
        /// <param name="store">store to search for dogs</param>
        /// <param name="dog">dog customer wishes to purchase</param>
        /// <param name="quant">number of dogs customer wishes to purchase</param>
        /// <returns>Item if store has it</returns>
        public Model.Item FindItem(StoreLocation store, Dog dog, int quant)
        {
            try{
                string add = FindStore(store.Address, store.Location).Address;
                string loc = FindStore(store.Address, store.Location).Location;
                Entity.Dog searchDog = (
                                        from Dog in _context.Dogs where 
                                        Dog.Breed == dog.Breed && Dog.Gender == dog.Gender.ToString()
                                        select Dog
                                        ).Single();
                Entity.DogStore dS = (
                                        from DogStore in _context.DogStores where 
                                        DogStore.StoreAddress == store.Address && DogStore.StoreName == store.Location
                                        select DogStore
                                        ).Single();
                Entity.Inventory inv = (
                                        from Inventory in _context.Inventories where
                                        Inventory.StoreId == dS.Id && Inventory.DogId == searchDog.ItemId
                                        select Inventory
                                        ).Single();
                if(inv.Quantity<quant) {
                    Console.WriteLine("Store doesn't have that many of that dog!");
                    throw new Exception();
                }
                else {
                    return new Model.OrderItem(new Dog(searchDog.Breed,searchDog.Gender.ToCharArray()[0],searchDog.Price,searchDog.ItemId),quant);
                }
            }
            catch(Exception){
                Log.Error("Item not found");
                return null;
            }
        }

        /// <summary>
        /// Old method to update item, replaced by AddItem method
        /// </summary>
        /// <param name="store">store that manager wishes to update</param>
        /// <param name="dog">dog to be updated</param>
        /// <param name="quant">quantity of dog to be added</param>
        /// <returns>Item updated</returns>
        public Model.Item UpdateItem(StoreLocation store, Dog dog, int quant)
        {
            try{
                Item itemToBeInc = FindItem(store, dog, quant);
                itemToBeInc.Quantity += quant;
                return itemToBeInc;
            }catch(Exception){
                Console.WriteLine("Item not found");
                return new Inventory(dog, quant);
            }
        }

        
        /// <summary>
        /// Finds a buyer in the database based on the phone number.
        /// </summary>
        /// <param name="phoneNumber">phoneNumber of the user you're looking for</param>
        /// <returns>Buyer if found, null otherwise</returns>
        public Model.DogBuyer FindBuyer(long phoneNumber)
        {
            try{
                Entity.DogBuyer dogBuyer = (
                                            from DogBuyer in _context.DogBuyers where 
                                            DogBuyer.PhoneNumber == phoneNumber
                                            select DogBuyer
                                            ).Single();
                return new Model.DogBuyer(dogBuyer.UserName, dogBuyer.UserAddress,dogBuyer.PhoneNumber);
            }catch(Exception e){
                Log.Debug(e.Message);
                return null;
            }
        }
        /// <summary>
        /// Find a buyer based on the name
        /// </summary>
        /// <param name="name">name to find</param>
        /// <returns>list of people with that name</returns>
        public List<Model.DogBuyer> FindBuyerByName(string name)
        {
            try{
                List<Entity.DogBuyer> dogBuyers = (
                                            from DogBuyer in _context.DogBuyers where 
                                            DogBuyer.UserName == name
                                            select DogBuyer
                                            ).ToList();
                List<Model.DogBuyer> dogBuyers1= new List<Model.DogBuyer>();
                foreach(Entity.DogBuyer dogBuyer in dogBuyers) dogBuyers1.Add(new Model.DogBuyer(dogBuyer.UserName, dogBuyer.UserAddress,dogBuyer.PhoneNumber));
                return dogBuyers1;
            }catch(Exception e){
                Log.Debug(e.Message);
                return null;
            }
        }
        /// <summary>
        /// Adds buyer to the database and returns added buyer
        /// </summary>
        /// <param name="buyer"> buyer to be added to the database</param>
        /// <returns>buyer added to the database</returns>
        public DogBuyer AddBuyer(DogBuyer buyer)
        {
            Entity.DogBuyer dogBuyer = new Entity.DogBuyer();
                    dogBuyer.UserName = buyer.Name;
                    dogBuyer.PhoneNumber = buyer.PhoneNumber;
                    dogBuyer.UserAddress = buyer.Address;
                    _context.DogBuyers.Add(dogBuyer);
                    _context.SaveChanges();
                    return buyer;
        }

        /// <summary>
        /// Finds manager in the database based on phone number
        /// </summary>
        /// <param name="phoneNumber">phone number to find the manager by</param>
        /// <returns>manager in the database if found and null otherwise</returns>
        public DogManager FindManager(long phoneNumber)
        {
            try{
                Entity.DogManager dogManager = (
                                            from DogManager in _context.DogManagers where 
                                            DogManager.PhoneNumber == phoneNumber
                                            select DogManager
                                            ).Single();
                return new Model.DogManager(dogManager.PhoneNumber,dogManager.UserAddress,dogManager.UserName);
            }catch(Exception e){
                Log.Debug(e.Message);
                return null;
            }
        }
        /// <summary>
        /// Adds a manager to the database
        /// </summary>
        /// <param name="manager">manager to be added</param>
        /// <returns>added manager</returns>
        public DogManager AddManager(DogManager manager)
        {
            Entity.DogManager dogManager = new Entity.DogManager();
                    dogManager.UserName = manager.Name;
                    dogManager.PhoneNumber = manager.PhoneNumber;
                    dogManager.UserAddress = manager.Address;
                    _context.DogManagers.Add(dogManager);
                    _context.SaveChanges();
                    return manager;
        }
        /// <summary>
        /// Adds order to the database
        /// </summary>
        /// <param name="dogOrder">dog order to be added to the database</param>
        /// <returns>order added to the database</returns>
        public DogOrder AddOrder(DogOrder dogOrder)
        {
            try{
                Entity.DogOrder dogOrd = new Entity.DogOrder();
                dogOrd.BuyerId = dogOrder.DogBuyer.PhoneNumber;
                dogOrd.StoreId = dogOrder.StoreLocation.id;
                dogOrd.DateOrder = dogOrder.OrderDate;
                dogOrd.Total = dogOrder.Total;
                _context.DogOrders.Add(dogOrd);
                _context.SaveChanges();
                Entity.OrderItem orderItem;
                dogOrd  = (
                            from DogOrder in _context.DogOrders where
                            DogOrder.BuyerId == dogOrder.DogBuyer.PhoneNumber &&
                            DogOrder.StoreId == dogOrder.StoreLocation.id &&
                            DogOrder.DateOrder == dogOrder.OrderDate &&
                            DogOrder.Total == dogOrder.Total
                            select DogOrder
                ).Single();
                foreach(Model.Item item in dogOrder.GetItems()){
                    Entity.Inventory inv = (
                                            from Inventory in _context.Inventories where
                                            Inventory.StoreId == dogOrder.StoreLocation.id && Inventory.DogId == item.Dog.id
                                            select Inventory
                                            ).Single();
                    inv.Quantity -= item.Quantity;
                    _context.SaveChanges();
                    orderItem = new Entity.OrderItem();
                    orderItem.DogId = item.Dog.id;
                    orderItem.OrderId = dogOrd.Id;
                    orderItem.Quantity = item.Quantity;
                    _context.OrderItems.Add(orderItem);
                    _context.SaveChanges();
                }
                return dogOrder;
            }
            catch(Exception e){
                Console.WriteLine("Something went wrong :(");
                Log.Error(e.Message);
                return null;
            }
        }

        /// <summary>
        /// Takes in a phone number of the user you're looking for and a special option
        /// parameter where user has inputted the query they wish to perform
        /// </summary>
        /// <param name="phoneNumber">phone number of User whose orders you wish to find</param>
        /// <param name="option">int where user has specified the query they wish to perform</param>
        /// <returns>List of orders user has purchased</returns>
        public List<Model.DogOrder> FindUserOrders(long phoneNumber, int option)
        {
            Model.DogBuyer dogBuyer = FindBuyer(phoneNumber);
            List<Entity.DogOrder> dogOrders = new List<Entity.DogOrder>();
            switch(option){
            case 1:
                dogOrders = (
                                            from DogOrder in _context.DogOrders where
                                            DogOrder.BuyerId == phoneNumber
                                            orderby DogOrder.DateOrder ascending
                                            select DogOrder
                                            ).ToList();
                break;
            case 2:
                dogOrders = (
                                            from DogOrder in _context.DogOrders where
                                            DogOrder.BuyerId == phoneNumber
                                            orderby DogOrder.DateOrder descending
                                            select DogOrder
                                            ).ToList();
                break;
            case 3:
                dogOrders = (
                                            from DogOrder in _context.DogOrders where
                                            DogOrder.BuyerId == phoneNumber
                                            orderby DogOrder.Total ascending
                                            select DogOrder
                                            ).ToList();
                break;
            case 4:
                dogOrders = (
                                            from DogOrder in _context.DogOrders where
                                            DogOrder.BuyerId == phoneNumber
                                            orderby DogOrder.Total descending
                                            select DogOrder
                                            ).ToList();
                break;
            default:
                return null;
            }
            Entity.DogStore dogStore;
            List<Entity.OrderItem> orderItems;
            List<Model.DogOrder> returnOrders = new List<Model.DogOrder>();
            Model.DogOrder returnOrder;
            Entity.Dog dog;
            foreach(Entity.DogOrder dogOrder in dogOrders){
                dogStore = (
                            from DogStore in _context.DogStores where
                            DogStore.Id == dogOrder.StoreId
                            select DogStore
                            ).Single();
                orderItems = (
                            from OrderItem in _context.OrderItems where
                            OrderItem.OrderId == dogOrder.Id
                            select OrderItem
                            ).ToList();
                returnOrder = new DogOrder(
                    dogBuyer,
                    dogOrder.Total,
                    new Model.StoreLocation(
                        dogStore.Id,
                        dogStore.StoreAddress,
                        dogStore.StoreName
                    )
                );
                returnOrder.OrderDate = dogOrder.DateOrder;
                foreach(Entity.OrderItem orderItem in orderItems){
                    dog = (
                            from Dog in _context.Dogs where
                            Dog.ItemId == orderItem.DogId
                            select Dog
                    ).Single();
                    returnOrder.AddItemToOrder(new Model.OrderItem(
                        new Model.Dog(
                            dog.Breed,
                            dog.Gender.ToCharArray()[0],
                            dog.Price
                        ),
                        orderItem.Quantity.Value
                    ));
                }
                returnOrders.Add(returnOrder);
            }
            return returnOrders;
        }

        /// <summary>
        /// Takes in address and storelocation of order history you're looking for and a special option
        /// parameter where user has inputted the query they wish to perform
        /// </summary>
        /// <param name="address">address of the store you're looking for orders of</param>
        /// <param name="location">name of the store you're looking for orders of</param>
        /// <param name="option">int where user has specified the query they wish to perform</param>
        /// <returns>List of orders purchased at store</returns>
        public List<DogOrder> FindStoreOrders(string address, string location, int option)
        {
            Model.StoreLocation store = FindStore(address,location);
            List<Entity.DogOrder> dogOrders = new List<Entity.DogOrder>();
            switch(option){
            case 1:
                dogOrders = (
                                            from DogOrder in _context.DogOrders where
                                            DogOrder.StoreId == store.id
                                            orderby DogOrder.DateOrder ascending
                                            select DogOrder
                                            ).ToList();
                break;
            case 2:
                dogOrders = (
                                            from DogOrder in _context.DogOrders where
                                            DogOrder.StoreId == store.id
                                            orderby DogOrder.DateOrder descending
                                            select DogOrder
                                            ).ToList();
                break;
            case 3:
                dogOrders = (
                                            from DogOrder in _context.DogOrders where
                                            DogOrder.StoreId == store.id
                                            orderby DogOrder.Total ascending
                                            select DogOrder
                                            ).ToList();
                break;
            case 4:
                dogOrders = (
                                            from DogOrder in _context.DogOrders where
                                            DogOrder.StoreId == store.id
                                            orderby DogOrder.Total descending
                                            select DogOrder
                                            ).ToList();
                break;
            default:
                return null;
            }
            Model.DogBuyer dogBuyer;
            List<Entity.OrderItem> orderItems;
            List<Model.DogOrder> returnOrders = new List<Model.DogOrder>();
            Model.DogOrder returnOrder;
            Entity.Dog dog;
            foreach(Entity.DogOrder dogOrder in dogOrders){
                dogBuyer = FindBuyer(dogOrder.BuyerId);
                orderItems = (
                            from OrderItem in _context.OrderItems where
                            OrderItem.OrderId == dogOrder.Id
                            select OrderItem
                            ).ToList();
                returnOrder = new DogOrder(
                    dogBuyer,
                    dogOrder.Total,
                    store
                );
                returnOrder.OrderDate = dogOrder.DateOrder;
                foreach(Entity.OrderItem orderItem in orderItems){
                    dog = (
                            from Dog in _context.Dogs where
                            Dog.ItemId == orderItem.DogId
                            select Dog
                    ).Single();
                    returnOrder.AddItemToOrder(new Model.OrderItem(
                        new Model.Dog(
                            dog.Breed,
                            dog.Gender.ToCharArray()[0],
                            dog.Price
                        ),
                        orderItem.Quantity.Value
                    ));
                }
                returnOrders.Add(returnOrder);
            }
            return returnOrders;
        }

        /// <summary>
        /// Simple method that just gets all buyers in the database
        /// </summary>
        /// <returns>List of all customers in the database</returns>
        public List<Model.DogBuyer> GetAllBuyers()
        {
            List<Entity.DogBuyer> dogBuyers = (
                                                from DogBuyer in _context.DogBuyers
                                                select DogBuyer
            ).ToList();
            List<Model.DogBuyer> returningDogBuyers = new List<Model.DogBuyer>();
            foreach(Entity.DogBuyer dogBuyer in dogBuyers){
                returningDogBuyers.Add(
                    new DogBuyer(
                        dogBuyer.UserName,
                        dogBuyer.UserAddress,
                        dogBuyer.PhoneNumber
                    )
                );
            }
            return returningDogBuyers;
        }
        /// <summary>
        /// Simple method that returns all the managers in the database
        /// </summary>
        /// <returns>List of dog managers</returns>

        public List<DogManager> GetAllDogManagers()
        {
            List<Entity.DogManager> dogManagers = (
                                                from DogManager in _context.DogManagers
                                                select DogManager
            ).ToList();
            List<Model.DogManager> returningDogManagers = new List<Model.DogManager>();
            foreach(Entity.DogManager dogManager in dogManagers){
                returningDogManagers.Add(
                    new DogManager(
                        dogManager.PhoneNumber,
                        dogManager.UserAddress,
                        dogManager.UserName
                    )
                );
            }
            return returningDogManagers;
        }
    }
}