using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
            DogId = i.DogId;
            Quantity = i.Quantity;
        }
        public InventoryVM(Inventory inv)
        {
            DogId = inv.DogId;
            Quantity = inv.Quantity;
            StoreLocationId = inv.StoreId;
            Store = inv.Store;
            
        }
        public List<string> DogStringList { get; set; }
        [Required]
        [Display(Name = "Dogs In Database")]
        public string DogString { get; set; }
        [Display(Name = "Dog Id")]
        public int DogId { get; set; }
        [Required]
        [Range(1, 100, ErrorMessage = "Must be between 1 and 100")]
        public int Quantity { get; set; }
        [Display(Name = "Store Id")]
        public int StoreLocationId { get; set; }
        public StoreLocation Store { get; set; }
       
        public string Breed { get; set; }

        public char Gender { get; set; }


        public double Price { get; set; }
    }
}
