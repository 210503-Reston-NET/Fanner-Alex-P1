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
            DogId = inv.Dog.Id;
            Quantity = inv.Quantity;
            StoreLocationId = inv.StoreId;
            Store = inv.Store;
            Breed = inv.Dog.Breed;
            Gender = inv.Dog.Gender;
            Price = inv.Dog.Price;
        }
        public int DogId { get; set; }
        [Required]
        [Range(1, 100, ErrorMessage = "Must be between 1 and 100")]
        public int Quantity { get; set; }
        public int StoreLocationId { get; set; }
        public StoreLocation Store { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z ]{4,}$", ErrorMessage = "Letters only please!")]
        public string Breed { get; set; }

        [Required]
        public char Gender { get; set; }

        [Required]
        [RegularExpression(@"^[0-9]{2,}.[0-9]{1,2}$", ErrorMessage = "Please enter price in Dollars.Cents form")]
        [Range(20,20000, ErrorMessage = "Please keep the price between 20 and 20,000 dollars")]
        public double Price { get; set; }
    }
}
