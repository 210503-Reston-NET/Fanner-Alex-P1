using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DSWebUI.Models
{
    public class OrderItemVM
    {
        public OrderItemVM() { }

        public string Breed { get; set; }
        [Display(Name = "Order Id")]
        public int OrderId { get; set; }
        public char Gender { get; set; }
        [Display(Name = "Dog Id")]
        public int DogId { get; set; }
        public double Price { get; set; }
        public List<string> DogStringList { get; set; }
        [Display(Name = "Store Inventory")]
        public string DogString { get; set; }
        [Required]
        [Range(1, 10, ErrorMessage = "Must be between 1 and 10")]
        public int Quantity { get; set; }
        
    }
}
