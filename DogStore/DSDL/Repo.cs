using DSModels;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
//using Entity = DSDL.Entities;

using Model = DSModels;

namespace DSDL
{
    /// <summary>
    /// Repository class to store data in SQL database
    /// </summary>
    public class Repo : IRepo
    {
        private List<Model.StoreLocation> _stores;

        private FannerDogsDBContext _context;

        public Repo(FannerDogsDBContext context)
        {
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
            try
            {
                StoreLocation storeLocation = (from sto in _context.StoreLocations
                                               where sto.Address == store.Address
                                               && sto.Location == store.Location
                                               select sto).Single();
                return null;
            }catch (Exception e) { }
            try
            {
                StoreLocation storeLo = new StoreLocation();
                storeLo.Location = store.Location;
                storeLo.Address = store.Address;
                var rand = new Random();
                storeLo.Id = rand.Next();
                _context.StoreLocations.Add(
                    storeLo
                );
                ManagesStore managesStore = new ManagesStore();

                _context.SaveChanges();
                Log.Information("Saved new store: " + store.Location + ", " + store.Address);
                managesStore.DogManagerId = dogManager.PhoneNumber;
                managesStore.StoreLocationId = storeLo.Id;
                _context.ManagesStores.Add(managesStore);
                _context.SaveChanges();
                Log.Information("Updated managesStore relationship");
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message + " error encountered in AddStoreLocation, this shouldn't happen");
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
            List<StoreLocation> StoreLocationList = (from storeLoc in _context.StoreLocations select storeLoc).ToList();
            Log.Information("Successfully retrieved stores");
            foreach (StoreLocation dS in StoreLocationList)
            {
                storeList.Add(new StoreLocation(dS.Id, dS.Address, dS.Location));
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
            try
            {
                StoreLocation dS = (
                                        from storeLoc in _context.StoreLocations
                                        where
                                        storeLoc.Address == address && storeLoc.Location == location
                                        select storeLoc
                                        ).Single();
                List<Inventory> iList = (
                                        from Inventory in _context.Inventories
                                        where
                                        Inventory.StoreId == dS.Id
                                        select Inventory
                                        ).ToList();
                List<Model.Item> itemList = new List<Model.Item>();
                foreach (Inventory i in iList)
                {
                    Dog dog = (
                                        from dog1 in _context.Dogs
                                        where
                                        dog1.Id == i.DogId
                                        select dog1
                    ).Single();
                    Log.Information("Here's the item I'm about to create");
                    Log.Information(dog.Breed);
                    Log.Information(dog.Gender.ToString());
                    Log.Information(dog.Price.ToString());
                    itemList.Add(new Model.Inventory(new Model.Dog(dog.Breed, dog.Gender, dog.Price), i.Quantity) { DogId = dog.Id});
                }
                return itemList;
            }
            catch (Exception e)
            {
                Log.Error(e.Message + " some issue with finding this inventory");
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
            try
            {
                Dog searchDog = (
                                        from dog1 in _context.Dogs
                                        where
                                        dog1.Breed == dog.Breed && dog1.Gender == dog.Gender
                                        select dog1
                                        ).Single();
                StoreLocation dS = (
                                        from storeLoc in _context.StoreLocations
                                        where
                                        storeLoc.Address == store.Address && storeLoc.Location == store.Location
                                        select storeLoc
                                        ).Single();

                try
                {
                    Inventory inv = (
                                        from inv1 in _context.Inventories
                                        where
                                        inv1.StoreId == dS.Id && inv1.DogId == searchDog.Id
                                        select inv1
                                        ).Single();
                    inv.Quantity += quant;
                    _context.SaveChanges();
                    Log.Information("Item found, quanty incremented: " + quant.ToString());
                    return newItem;
                }
                catch (Exception e)
                {
                    Log.Information(e.Message + "dog found but not inventory, adding dog to store's inventory");
                    Inventory inventory = new Inventory();
                    inventory.DogId = searchDog.Id;
                    inventory.Quantity = quant;
                    inventory.StoreId = dS.Id;
                    _context.Inventories.Add(inventory);
                    _context.SaveChanges();
                    return newItem;
                }
            }
            catch (Exception e)
            {
                Log.Information(e.Message + "Dog not found, creating new dog");
                Dog newDog = new Dog();
                //newDog.ItemId = new Random().Next();
                newDog.Breed = dog.Breed;
                newDog.Gender = dog.Gender;
                newDog.Price = dog.Price;
                _context.Dogs.Add(newDog);
                _context.SaveChanges();
                Dog searchDog = newDog;
                StoreLocation dS = (
                                        from storeLoc in _context.StoreLocations
                                        where
                                        storeLoc.Address == store.Address && storeLoc.Location == store.Location
                                        select storeLoc
                                        ).Single();
                Inventory inventory = new Inventory();
                inventory.DogId = searchDog.Id;
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
        public Model.StoreLocation FindStore(string address, string location)
        {
            //StoreLocation store = new StoreLocation(address, location);
            return GetAllStoreLocations().First(stor => stor.Address == address && stor.Location == location);
            //from StoreLocation in _context.StoreLocations where
        }

        public Model.StoreLocation FindStore(int storeId)
        {
            //StoreLocation store = new StoreLocation(address, location);
            try { return GetAllStoreLocations().First(stor => stor.Id == storeId); }
            catch (Exception) {
                Log.Error("Unable to find store with Id: " + storeId.ToString());
                return null; }
            //from StoreLocation in _context.StoreLocations where
        }
        /// <summary>
        /// Finds a store you're looking for and removes it NOT IMPLEMENTED
        /// </summary>
        /// <param name="address"> Address of the store you want to remove.</param>
        /// <param name="location"> Name of the store you want to remove.</param>
        /// <returns> Store which was removed from memory.</returns>
        public Model.StoreLocation RemoveStore(int id)
        {
            StoreLocation storeDel = _context.StoreLocations.First(sto => sto.Id == id);
            _context.StoreLocations.Remove(storeDel);
            _context.SaveChanges();
            return storeDel;
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
            try
            {
                string add = FindStore(store.Address, store.Location).Address;
                string loc = FindStore(store.Address, store.Location).Location;
                Dog searchDog = (
                                        from dog1 in _context.Dogs
                                        where
                                        dog1.Breed == dog.Breed && dog1.Gender == dog.Gender
                                        select dog1
                                        ).Single();
                StoreLocation dS = (
                                        from storeLoc in _context.StoreLocations
                                        where
                                        storeLoc.Address == store.Address && storeLoc.Location == store.Location
                                        select storeLoc
                                        ).Single();
                Inventory inv = (
                                        from invent in _context.Inventories
                                        where
                                        invent.StoreId == dS.Id && invent.DogId == searchDog.Id
                                        select invent
                                        ).Single();
                if (inv.Quantity < quant)
                {
                    Log.Error("Store doesn't have that many of that dog!");
                    throw new Exception();
                }
                else
                {
                    return new Model.OrderItem(new Dog(searchDog.Breed, searchDog.Gender, searchDog.Price), quant);
                }
            }
            catch (Exception)
            {
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
            try
            {
                Item itemToBeInc = FindItem(store, dog, quant);
                itemToBeInc.Quantity += quant;
                Log.Information("Item successfully stocked for following store, dog breed: " + store.Location + " " + store.Address + ", " + dog.Breed);
                return itemToBeInc;
            }
            catch (Exception)
            {
                Log.Error("Item not found");
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
            try
            {
                DogBuyer dogBuyer = (
                                            from dogBuy in _context.DogBuyers
                                            where
                                            dogBuy.PhoneNumber == phoneNumber
                                            select dogBuy
                                            ).Single();
                Log.Information("Buyer with id: " + phoneNumber.ToString() + " retrieved from DB");
                return new Model.DogBuyer(dogBuyer.Name, dogBuyer.Address, dogBuyer.PhoneNumber);
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
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
            try
            {
                List<DogBuyer> dogBuyers = (
                                            from dogBuy in _context.DogBuyers
                                            where
                                            dogBuy.Name == name
                                            select dogBuy
                                            ).ToList();
                List<Model.DogBuyer> dogBuyers1 = new List<Model.DogBuyer>();
                foreach (DogBuyer dogBuyer in dogBuyers) dogBuyers1.Add(new Model.DogBuyer(dogBuyer.Name, dogBuyer.Address, dogBuyer.PhoneNumber));
                return dogBuyers1;
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
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
            DogBuyer dogBuyer = new DogBuyer();
            dogBuyer.Name = buyer.Name;
            dogBuyer.PhoneNumber = buyer.PhoneNumber;
            dogBuyer.Address = buyer.Address;
            _context.DogBuyers.Add(dogBuyer);
            _context.SaveChanges();
            Log.Information("Buyer with number: " + buyer.PhoneNumber.ToString() + " added to DB");
            return buyer;
        }

        /// <summary>
        /// Finds manager in the database based on phone number
        /// </summary>
        /// <param name="phoneNumber">phone number to find the manager by</param>
        /// <returns>manager in the database if found and null otherwise</returns>
        public DogManager FindManager(long phoneNumber)
        {
            try
            {
                DogManager dogManager = (
                                            from dogMan in _context.DogManagers
                                            where
                                            dogMan.PhoneNumber == phoneNumber
                                            select dogMan
                                            ).Single();
                Log.Information("Manager with number " + phoneNumber.ToString() + " retrieved from the db");
                return new Model.DogManager(dogManager.PhoneNumber, dogManager.Address, dogManager.Name);
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
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
            DogManager dogManager = new DogManager();
            dogManager.Name = manager.Name;
            dogManager.PhoneNumber = manager.PhoneNumber;
            dogManager.Address = manager.Address;
            _context.DogManagers.Add(dogManager);
            _context.SaveChanges();
            Log.Information("Manager added to the database with number: " + manager.PhoneNumber.ToString());
            return manager;
        }

        /// <summary>
        /// Adds order to the database
        /// </summary>
        /// <param name="dogOrder">dog order to be added to the database</param>
        /// <returns>order added to the database</returns>
        public DogOrder AddOrder(DogOrder dogOrder)
        {
            try
            {
                DogOrder dogOrd = new DogOrder();
                dogOrd.BuyerId = dogOrder.DogBuyer.PhoneNumber;
                dogOrd.StoreId = dogOrder.StoreLocation.Id;
                dogOrd.OrderDate = dogOrder.OrderDate;
                dogOrd.Total = dogOrder.Total;
                _context.DogOrders.Add(dogOrd);
                _context.SaveChanges();
                OrderItem orderItem;
                dogOrd = (
                            from dogOr in _context.DogOrders
                            where
                                dogOr.BuyerId == dogOrder.DogBuyer.PhoneNumber &&
                                dogOr.StoreId == dogOrder.StoreLocation.Id &&
                                dogOr.OrderDate == dogOrder.OrderDate &&
                                dogOr.Total == dogOrder.Total
                            select dogOr
                ).Single();
                Log.Information("Successfully added order");
                return dogOrd;
            }
            catch (Exception e)
            {
                
                Log.Error(e.Message);
                return dogOrder;
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
            Log.Information("Beginning to search for orders from BuyerId: " + phoneNumber.ToString());
            Model.DogBuyer dogBuyer = FindBuyer(phoneNumber);
            List<DogOrder> dogOrders = new List<DogOrder>();
            switch (option)
            {
                case 1:
                    dogOrders = (
                                                from dogOrd1 in _context.DogOrders
                                                where
                                                dogOrd1.BuyerId == phoneNumber
                                                orderby dogOrd1.OrderDate ascending
                                                select dogOrd1
                                                ).ToList();
                    break;

                case 2:
                    dogOrders = (
                                                from dogOrd1 in _context.DogOrders
                                                where
                                                dogOrd1.BuyerId == phoneNumber
                                                orderby dogOrd1.OrderDate descending
                                                select dogOrd1
                                                ).ToList();
                    break;

                case 3:
                    dogOrders = (
                                                from dogOrd1 in _context.DogOrders
                                                where
                                                 dogOrd1.BuyerId == phoneNumber
                                                orderby dogOrd1.Total ascending
                                                select dogOrd1
                                                ).ToList();
                    break;

                case 4:
                    dogOrders = (
                                                from dogOrd1 in _context.DogOrders
                                                where
                                                dogOrd1.BuyerId == phoneNumber
                                                orderby dogOrd1.Total descending
                                                select dogOrd1
                                                ).ToList();
                    break;

                default:
                    return null;
            }
            StoreLocation storeLocation;
            List<OrderItem> orderItems;
            List<DogOrder> returnOrders = new List<DogOrder>();
            DogOrder returnOrder;
            Dog dog;
            foreach (DogOrder dogOrder in dogOrders)
            {
                storeLocation = (
                            from storeLoc in _context.StoreLocations
                            where
                            storeLoc.Id == dogOrder.StoreId
                            select storeLoc
                            ).Single();
                orderItems = (
                            from orderIt in _context.OrderItems
                            where
                            orderIt.OrderId == dogOrder.Id
                            select orderIt
                            ).ToList();
                returnOrder = new DogOrder(
                    dogBuyer,
                    dogOrder.Total,
                    storeLocation
                );
                returnOrder.OrderDate = dogOrder.OrderDate;
                returnOrder.StoreId = storeLocation.Id;
                returnOrder.Id = dogOrder.Id;
                foreach (OrderItem orderItem in orderItems)
                {
                    dog = (
                            from dog1 in _context.Dogs
                            where
                            dog1.Id == orderItem.DogId
                            select dog1
                    ).Single();
                    returnOrder.AddItemToOrder(new OrderItem(
                        dog,
                        orderItem.Quantity
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
            Model.StoreLocation store = FindStore(address, location);
            List<DogOrder> dogOrders = new List<DogOrder>();
            Log.Information("Beginning to search for orders from Store: " + location + " " + address);
            switch (option)
            {
                case 1:
                    dogOrders = (
                                                from dogOrd1 in _context.DogOrders
                                                where
                                                dogOrd1.StoreId == store.Id
                                                orderby dogOrd1.OrderDate ascending
                                                select dogOrd1
                                                ).ToList();
                    break;

                case 2:
                    dogOrders = (
                                                from dogOrd1 in _context.DogOrders
                                                where
                                                dogOrd1.StoreId == store.Id
                                                orderby dogOrd1.OrderDate descending
                                                select dogOrd1
                                                ).ToList();
                    break;

                case 3:
                    dogOrders = (
                                                from dogOrd1 in _context.DogOrders
                                                where
                                                dogOrd1.StoreId == store.Id
                                                orderby dogOrd1.Total ascending
                                                select dogOrd1
                                                ).ToList();
                    break;

                case 4:
                    dogOrders = (
                                                from dogOrd1 in _context.DogOrders
                                                where
                                                dogOrd1.StoreId == store.Id
                                                orderby dogOrd1.Total descending
                                                select dogOrd1
                                                ).ToList();
                    break;

                default:
                    return null;
            }
            DogBuyer dogBuyer;
            List<OrderItem> orderItems;
            List<DogOrder> returnOrders = new List<DogOrder>();
            DogOrder returnOrder;
            Dog dog;
            foreach (DogOrder dogOrder in dogOrders)
            {
                dogBuyer = FindBuyer(dogOrder.BuyerId);
                orderItems = (
                            from orderIt in _context.OrderItems
                            where
                            orderIt.OrderId == dogOrder.Id
                            select orderIt
                            ).ToList();
                returnOrder = new DogOrder(
                    dogBuyer,
                    dogOrder.Total,
                    store
                );
                returnOrder.OrderDate = dogOrder.OrderDate;
                returnOrder.Id = dogOrder.Id;
                returnOrder.BuyerId = dogOrder.BuyerId;
                foreach (OrderItem orderItem in orderItems)
                {
                    dog = (
                            from dog1 in _context.Dogs
                            where
                            dog1.Id == orderItem.DogId
                            select dog1
                    ).Single();
                    returnOrder.AddItemToOrder(new OrderItem(
                        dog,
                        orderItem.Quantity
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
            List<DogBuyer> dogBuyers = (
                                                from dogBuy in _context.DogBuyers
                                                select dogBuy
            ).ToList();
            List<DogBuyer> returningDogBuyers = new List<DogBuyer>();
            foreach (DogBuyer dogBuyer in dogBuyers)
            {
                returningDogBuyers.Add(
                    dogBuyer
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
            List<DogManager> dogManagers = (
                                                from DogManager in _context.DogManagers
                                                select DogManager
            ).ToList();
            List<DogManager> returningDogManagers = new List<DogManager>();
            foreach (DogManager dogManager in dogManagers)
            {
                returningDogManagers.Add(
                    dogManager
                );
            }
            return returningDogManagers;
        }
        /// <summary>
        /// From a manager's number, get related stores
        /// </summary>
        /// <param name="phonenumber">Manager's phone number</param>
        /// <returns>List of Manager's stores as List<StoreLocation></StoreLocation></returns>
        public List<StoreLocation> GetManagerStores(long phonenumber)
        {
            try {
                List<ManagesStore> manages = (
                                                from mS in _context.ManagesStores
                                                where mS.DogManagerId == phonenumber
                                                select mS
                    ).ToList();
                
                

                List<StoreLocation> stores = (
                                                from sL in _context.StoreLocations
                                                select sL
                ).ToList();
                List<StoreLocation> returnStores = new List<StoreLocation>();
                foreach (StoreLocation s in stores)
                {
                    foreach(ManagesStore m in manages)
                    {
                        if (m.StoreLocationId == s.Id) returnStores.Add(s);
                    }
                }
                Log.Information("Got manager's stores for: " + phonenumber.ToString());
                return returnStores;
            }
            catch(Exception e)
            {
                return new List<StoreLocation>();
            }
        }
        /// <summary>
        /// Retrieve a dog by its Id
        /// </summary>
        /// <param name="dogId">Unique Id for breed and gender of dog</param>
        /// <returns>Dog if found null otherwise</returns>
        public Dog FindDog(int dogId)
        {
            try
            {
                return (from d in _context.Dogs
                        where d.Id == dogId
                        select d).Single();
                Log.Information("Found dog with id: " + dogId.ToString());
            }
            catch (Exception)
            {
                Log.Error("Dog Not found in DB");
                return null;
            }
        }
        /// <summary>
        /// Get all Order Items related to a certain order Id
        /// </summary>
        /// <param name="id">OrderId of order related to items</param>
        /// <returns>List<OrderItem> for the order, new list if not found</returns>
        public List<OrderItem> GetOrderItems(int id)
        {
            try
            {
                return (from dO in _context.OrderItems
                        where dO.OrderId == id
                        select dO).ToList();
                Log.Information("Retrieved order items for " + id.ToString());
            }catch(Exception e)
            {
                Log.Information("New List for customer to start order");
                return new List<OrderItem>();
            }
        }
        /// <summary>
        /// Updates an order, mainly used for incrementing total and setting the correct date of 
        /// an order
        /// </summary>
        /// <param name="dogOrder">Order to be updated with its fields</param>
        /// <returns>DogOrder if it is found, null otherwise</returns>
        public DogOrder UpdateOrder(DogOrder dogOrder)
        {
            try
            {
                
                _context.DogOrders.Update(dogOrder);
                _context.SaveChanges();
                Log.Information("Order updated in db");
                return dogOrder;
            }
            catch (Exception)
            {
                Log.Error("Tried to update order and failed");
                return null;
            }
        }
        /// <summary>
        /// Get an Order with its unique id
        /// </summary>
        /// <param name="id">Id of the order to be found</param>
        /// <returns>Order matching the id if found, null otherwise</returns>
        public DogOrder GetOrder(int id)
        {
            try
            {
                DogOrder dogOrder = (from dO in _context.DogOrders
                                     where dO.Id == id
                                     select dO).Single();
                Log.Information("Retrieved order with id: " + id.ToString());
                return dogOrder;
            }
            catch (Exception)
            {
                Log.Error("Order not found");
                return null;
            }
        }

        /// <summary>
        /// Returns order Item to be ordered, returns null if it isn't found
        /// </summary>
        /// <param name="orderItem"> OrderItem to add to order</param>
        /// <param name="storeId">store that one is ordering from</param>
        /// <returns>OrderItem if added, null if already in order</returns>
        public OrderItem AddOrderItem(OrderItem orderItem, int storeId)
        {
            try {
                OrderItem checkifinDB = (from oI in _context.OrderItems
                                         where oI.OrderId == orderItem.OrderId &&
                                         oI.DogId == orderItem.DogId
                                         select oI).Single();
                Log.Error("User tried to add dog that was already in order, dogId: " + orderItem.DogId.ToString());
                return null;
            }
            catch (Exception) {
                _context.OrderItems.Add(orderItem);
                _context.SaveChanges();
                DecInv(storeId, orderItem.DogId, orderItem.Quantity);
                return orderItem;
            }
        }
        /// <summary>
        /// Find dog by breed and gender
        /// </summary>
        /// <param name="breed">Breed of dog to be found</param>
        /// <param name="gender">Gender of dog to be found</param>
        /// <returns>Dog in the database, null otherwise</returns>
        public Dog FindDog(string breed, char gender)
        {
            try
            {
                Dog findDog = (from d in _context.Dogs
                               where d.Breed == breed &&
                               d.Gender == gender
                               select d).Single();
                Log.Information("Found the dog of breed and gender: " + breed +", " + gender.ToString());
                return findDog;
            }
            catch (Exception)
            {
                Log.Error("Dog Not Found");
                return null;
            }
        }
        /// <summary>
        /// Method that decrements inventory for orders that have been purchased 
        /// </summary>
        /// <param name="storeId">Id of the store to decrement from</param>
        /// <param name="dogId">DogId of dog to be decremented</param>
        /// <param name="quant"></param>
        /// <returns>Inventory if found, null otherwise</returns>
        public Inventory DecInv(int storeId, int dogId, int quant)
        {
            try
            {
                Inventory itemDec = (from inv in _context.Inventories
                                     where inv.StoreId == storeId &&
                                     inv.DogId == dogId
                                     select inv).Single();
                itemDec.Quantity -= quant;
                if (itemDec.Quantity == 0)
                {
                    Log.Information("Removed item with dogId: " + dogId.ToString() + "from storeId: " + storeId.ToString() + " beacause quantity was 0");
                    _context.Inventories.Remove(itemDec);
                    _context.SaveChanges();

                }
                return itemDec;
            }
            catch (Exception)
            {
                Console.WriteLine("Item not found");
                return null;
            }
        }
        /// <summary>
        /// Method that gets the whole list of dogs in the database
        /// </summary>
        /// <returns>List<Dog> in DB</returns>
        public List<Dog> GetDogs()
        {
            try
            {
                Log.Debug("Calling database to retrieve dogs");
                return (from d in _context.Dogs
                        select d).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }
        /// <summary>
        /// Add inputted dog to the list of dogs in the database
        /// </summary>
        /// <param name="dog">dog to be added</param>
        /// <returns>dog added, null if already found</returns>
        public Dog AddDog(Dog dog)
        {
            try {
                Dog checkDogInDB = (from d in _context.Dogs
                         where d.Breed == dog.Breed &&
                         d.Gender == dog.Gender
                         select d).Single();
                Log.Error("Dog Should not be in Database, sending user back");
                return null;
            }
            catch (Exception)
            {
                Log.Information("Adding dog with breed and gender: " + dog.Breed + ", " + dog.Gender.ToString());
                _context.Dogs.Add(dog);
                _context.SaveChanges();
                return dog;
            }
        }
    }
}