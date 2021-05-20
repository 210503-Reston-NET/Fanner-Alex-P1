using System;
using System.Collections.Generic;

#nullable disable

namespace DSDL.Entities
{
    public partial class DogManager
    {
        public DogManager()
        {
            ManagesStores = new HashSet<ManagesStore>();
        }

        public long PhoneNumber { get; set; }
        public string UserAddress { get; set; }
        public string UserName { get; set; }

        public virtual ICollection<ManagesStore> ManagesStores { get; set; }
    }
}
