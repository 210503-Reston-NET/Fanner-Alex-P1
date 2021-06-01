//System.Security.Cryptography.X509Certificates;
using DSModels;
using System.Collections.Generic;

namespace DSBL
{
    public interface IStoreLocationBL
    {
        List<StoreLocation> GetAllStoreLocations();

        StoreLocation AddStoreLocation(StoreLocation store, DogManager dogManager);

        List<Item> GetStoreInventory(string address, string location);

        StoreLocation GetStore(string address, string location);

        StoreLocation RemoveStore(int id);

        Item FindItem(StoreLocation store, Dog dog, int quant);

        Item UpdateItem(StoreLocation store, Dog dog, int quant);

        Item AddItem(StoreLocation store, Dog dog, int quant);
        StoreLocation GetStore(int Id);
        Dog GetDog(int Id);
        Dog FindDog(string breed, char gender);
        List<Dog> GetDogs();
        Dog AddDog(Dog d);
        
    }
}