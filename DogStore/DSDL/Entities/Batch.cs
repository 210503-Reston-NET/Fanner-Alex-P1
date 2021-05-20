using System;
using System.Collections.Generic;

#nullable disable

namespace DSDL.Entities
{
    public partial class Batch
    {
        public int Id { get; set; }
        public int? AssociateId { get; set; }
        public int? TrainerId { get; set; }

        public virtual Associate Associate { get; set; }
        public virtual Trainer Trainer { get; set; }
    }
}
