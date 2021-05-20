using System;
using System.Collections.Generic;

#nullable disable

namespace DSDL.Entities
{
    public partial class Associate
    {
        public Associate()
        {
            Batches = new HashSet<Batch>();
        }

        public int Id { get; set; }
        public string AssociateName { get; set; }
        public string AssociateLocale { get; set; }
        public int RevaPoints { get; set; }

        public virtual ICollection<Batch> Batches { get; set; }
    }
}
