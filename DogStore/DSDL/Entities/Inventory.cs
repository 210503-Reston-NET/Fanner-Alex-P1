using System;
using System.Collections.Generic;

#nullable disable

namespace DSDL.Entities
{
    public partial class Inventory
    {
        public int Id { get; set; }
        public int? StoreId { get; set; }
        public long? DogId { get; set; }
        public int? Quantity { get; set; }

        public virtual Dog Dog { get; set; }
        public virtual DogStore Store { get; set; }
    }
}
