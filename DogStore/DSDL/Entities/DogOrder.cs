using System;
using System.Collections.Generic;

#nullable disable

namespace DSDL.Entities
{
    public partial class DogOrder
    {
        public DogOrder()
        {
            OrderItems = new HashSet<OrderItem>();
        }

        public int Id { get; set; }
        public DateTime DateOrder { get; set; }
        public long BuyerId { get; set; }
        public double Total { get; set; }
        public int StoreId { get; set; }

        public virtual DogBuyer Buyer { get; set; }
        public virtual DogStore Store { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}
