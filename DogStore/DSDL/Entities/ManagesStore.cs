using System;
using System.Collections.Generic;

#nullable disable

namespace DSDL.Entities
{
    public partial class ManagesStore
    {
        public int Id { get; set; }
        public long? ManagerId { get; set; }
        public int? StoreId { get; set; }

        public virtual DogManager Manager { get; set; }
        public virtual DogStore Store { get; set; }
    }
}
