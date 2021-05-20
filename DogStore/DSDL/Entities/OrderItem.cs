using System;
using System.Collections.Generic;

#nullable disable

namespace DSDL.Entities
{
    public partial class OrderItem
    {
        public int Id { get; set; }
        public long? DogId { get; set; }
        public int? OrderId { get; set; }
        public int? Quantity { get; set; }

        public virtual Dog Dog { get; set; }
        public virtual DogOrder Order { get; set; }
    }
}
