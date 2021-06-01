using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DSWebUI.Models
{
    public class DogVM
    {
        public DogVM() { }
        public int Id { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z ]{3,}$", ErrorMessage = "Letters only please!")]
        public string Breed { get; set; }

        [Required]
        public char Gender { get; set; }
        [Required]
        [RegularExpression(@"^[0-9]{2,}.[0-9]{1,2}$", ErrorMessage = "Please enter price in Dollars.Cents form")]
        [Range(20, 20000, ErrorMessage = "Please keep the price between 20 and 20,000 dollars")]
        public double Price { get; set; }
    }
}
