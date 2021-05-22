using System.Collections.Generic;

namespace DSModels
{
    /// <summary>
    /// Class representing a store manager with their name and
    /// the stores that they manage.
    /// </summary>
    public class DogManager : UserInterface
    {
        /// <summary>
        /// List of the stores that the manager manages.
        /// </summary>
        private List<StoreLocation> _managedStores;

        public DogManager()
        {
        }

        public DogManager(long phoneNumber, string address, string name)
        {
            this.PhoneNumber = phoneNumber;
            this.Address = address;
            this.Name = name;
            _managedStores = new List<StoreLocation>();
        }

        /// <summary>
        /// String with manager's name.
        /// </summary>
        /// <value></value>
        public string Name { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// <value></value>
        public string Address { get; set; }

        public long PhoneNumber { get; set; }

        public StoreLocation AddStore(StoreLocation store)
        {
            _managedStores.Add(store);
            return store;
        }

        public List<StoreLocation> GetManagedStores()
        {
            return this._managedStores;
        }
    }
}