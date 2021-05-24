using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DSModels;
namespace DSWebUI.Models
{
    /// <summary>
    /// Presents information concerning Dog Managers
    /// </summary>
    public class DogManagerVM
    {
        public DogManagerVM() { }
        public DogManagerVM(DogManager dM)
        {
            Name = dM.Name;
            Address = dM.Address;
            PhoneNumber = dM.PhoneNumber;
        }
        public string Name { get; set; }

        public string Address { get; set; }

        public long PhoneNumber { get; set; }
    }
}
