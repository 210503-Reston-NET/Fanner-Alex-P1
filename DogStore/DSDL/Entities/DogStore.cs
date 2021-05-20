using System;
using System.Collections.Generic;

#nullable disable

namespace DSDL.Entities
{
    public partial class DogStore
    {
        public DogStore()
        {
            DogOrders = new HashSet<DogOrder>();
            Inventories = new HashSet<Inventory>();
            ManagesStores = new HashSet<ManagesStore>();
        }

        public int Id { get; set; }
        public string StoreAddress { get; set; }
        public string StoreName { get; set; }

        public virtual ICollection<DogOrder> DogOrders { get; set; }
        public virtual ICollection<Inventory> Inventories { get; set; }
        public virtual ICollection<ManagesStore> ManagesStores { get; set; }
    }
}
