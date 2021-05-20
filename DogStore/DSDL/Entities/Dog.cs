using System;
using System.Collections.Generic;

#nullable disable

namespace DSDL.Entities
{
    public partial class Dog
    {
        public Dog()
        {
            Inventories = new HashSet<Inventory>();
            OrderItems = new HashSet<OrderItem>();
        }

        public long ItemId { get; set; }
        public string Breed { get; set; }
        public string Gender { get; set; }
        public double Price { get; set; }

        public virtual ICollection<Inventory> Inventories { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}
