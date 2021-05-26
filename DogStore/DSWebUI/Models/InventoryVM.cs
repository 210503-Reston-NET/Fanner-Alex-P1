using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DSModels;
namespace DSWebUI.Models
{
    public class InventoryVM
    {
        public InventoryVM() { }
        public InventoryVM(Item i)
        {

        }
        public InventoryVM(Inventory inv)
        {
            DogId = inv.Id;
            Quantity = inv.Quantity;
            StoreLocationId = inv.StoreId;
            Store = inv.Store;
        }
        public int DogId { get; set; }
        public int Quantity { get; set; }
        public int StoreLocationId { get; set; }
        public StoreLocation Store { get; set; }
    }
}
