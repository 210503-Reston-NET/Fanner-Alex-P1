using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DSWebUI.Models
{
    public class OrderItemVM
    {
        public OrderItemVM() { }
        public string Breed { get; set; }
        public int OrderId { get; set; }
        public char Gender { get; set; }
        public int DogId { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }

    }
}
