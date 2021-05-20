using System;
using System.Collections.Generic;

#nullable disable

namespace DSDL.Entities
{
    public partial class Trainer
    {
        public Trainer()
        {
            Batches = new HashSet<Batch>();
        }

        public int Id { get; set; }
        public string TrainerName { get; set; }
        public string Campus { get; set; }
        public int CaffeineLevel { get; set; }

        public virtual ICollection<Batch> Batches { get; set; }
    }
}
