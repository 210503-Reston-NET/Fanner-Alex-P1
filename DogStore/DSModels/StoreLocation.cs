using System.Collections.Generic;

namespace DSModels
{
    /// <summary>
    /// Class representing a store with address, location, and inventory
    /// </summary>
    public class StoreLocation
    {
        /// <summary>
        /// List of items representing the store's inventory
        /// </summary>
        private List<Item> _inventory;

        /// <summary>
        /// id for db structure
        /// </summary>
        //private int id;
        /// <summary>
        /// Basic constructor with address and location.
        /// </summary>
        /// <param name="address">string representing store's address</param>
        /// <param name="location">string representing location's address</param>
        public StoreLocation(string address, string location)
        {
            this.Address = address;
            this.Location = location;
            this._inventory = new List<Item>();
            //this._inventory.Add(new Item(new Dog("Blue Heeler", "Female", 1000.00),1));
        }

        public StoreLocation(int id, string address, string location) : this(address, location)
        {
            this.Id = id;
        }

        public StoreLocation()
        {
        }

        public int Id { get; set; }

        /// <summary>
        /// String representing the address of the store.
        /// </summary>
        /// <value></value>
        public string Address { get; set; }

        /// <summary>
        /// String representing the location of the store.
        /// </summary>
        /// <value></value>
        public string Location { get; set; }

        /// <summary>
        /// Adds an item to the inventory
        /// </summary>
        /// <param name="item"> item to be added to inventory</param>
        /// <returns>added item</returns>
        public Item AddItem(Item item)
        {
            _inventory.Add(item);
            return item;
        }

        /// <summary>
        /// Get the inventory of the store
        /// </summary>
        /// <returns>List of items in store's inventory</returns>
        public List<Item> GetInventory()
        {
            return _inventory;
        }

        public void SetInventory(List<Item> iList)
        {
            _inventory = iList;
        }

        /// <summary>
        /// Overriding the ToString() method to return basic information of the store.
        /// </summary>
        /// <returns>string representing the store's information</returns>
        public override string ToString()
        {
            return "Address: " + this.Address + " Location: " + this.Location;
        }

        public bool Equals(StoreLocation store)
        {
            return this.Address.Equals(store.Address) && this.Location.Equals(store.Location);
        }
    }
}