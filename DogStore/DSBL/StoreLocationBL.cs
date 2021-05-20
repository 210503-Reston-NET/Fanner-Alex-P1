using System.Collections.Generic;
using DSDL;
using DSModels;
using Entity = DSDL.Entities;
namespace DSBL
{
    /// <summary>
    /// Class representing the BL for storing store locations.
    /// </summary>
    public class StoreLocationBL:IStoreLocationBL
    {
        private Repo _repoDS;
        public StoreLocationBL(Entity.FannerDogsDBContext context ){
            _repoDS =  new Repo(context);
        }
        /// <summary>
        /// Method which accesses the data layer to get all stores in memory.
        /// </summary>
        /// <returns> List of stores which are accessed through the data layer.</returns>
        public List<StoreLocation> GetAllStoreLocations(){
            return _repoDS.GetAllStoreLocations();
        }
        /// <summary>
        /// Method which accesses the data layer to add a store to the database.
        /// </summary>
        /// <param name="store"> Store to add to the database through the data layer.</param>
        /// <returns> Store location which was added.</returns>
        public StoreLocation AddStoreLocation(StoreLocation store, DogManager dogManager){
            return _repoDS.AddStoreLocation(store,dogManager);
        }
        /// <summary>
        /// Method which returns an inventory given a store address and location, 
        /// accessing the data layer to find it.
        /// </summary>
        /// <param name="address"> Address of the store from which you wish to find the inventory</param>
        /// <param name="location"> Store's name</param>
        /// <returns> List of items representing the store's inventory</returns>
        public List<Item> GetStoreInventory(string address, string location)
        {
            return _repoDS.GetStoreInventory(address, location);
        }
        /// <summary>
        /// Method that accesses data layer in order to get a requested store.
        /// </summary>
        /// <param name="address"> Adrress of the requested store</param>
        /// <param name="location"> Location name of the requested store</param>
        /// <returns> StoreLocation requested</returns>
        public StoreLocation GetStore(string address, string location){
            return _repoDS.FindStore(address, location);
        }
        /// <summary>
        /// Method that removes the requested store from the database.
        /// </summary>
        /// <param name="address">Address of the store to be removed.</param>
        /// <param name="location">Location name of the store to be removed.</param>
        /// <returns> StoreLocation that was removed</returns>
        public StoreLocation RemoveStore(string address, string location)
        {
            return _repoDS.RemoveStore(address, location);
        }

        public Item FindItem(StoreLocation store, Dog dog, int quant)
        {
            return _repoDS.FindItem(store, dog, quant);
        }

        public Item UpdateItem(StoreLocation store, Dog dog, int quant)
        {
            return _repoDS.UpdateItem(store, dog, quant);
        }

        public Item AddItem(StoreLocation store, Dog dog, int quant)
        {
            return _repoDS.AddItem(store, dog, quant);
        }
    }
}