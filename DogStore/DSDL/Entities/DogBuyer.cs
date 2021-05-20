using System;
using System.Collections.Generic;

#nullable disable

namespace DSDL.Entities
{
    public partial class DogBuyer
    {
        public DogBuyer()
        {
            DogOrders = new HashSet<DogOrder>();
        }

        public long PhoneNumber { get; set; }
        public string UserAddress { get; set; }
        public string UserName { get; set; }

        public virtual ICollection<DogOrder> DogOrders { get; set; }
    }
}
