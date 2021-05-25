using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        [Required]
        [Display(Name = "Name")]
        [RegularExpression(@"^[a-zA-Z]{2,}\s[a-zA-Z]{1,}$", ErrorMessage = "Please use the Format: Firstname Lastname")]
        public string Name { get; set; }

        [Display(Name = "Address")]
        [Required]
        [RegularExpression(@"^[\w\s]+,\s\w{2}$", ErrorMessage = "Please use the Format: City Name, ST")]
        public string Address { get; set; }

        [Display(Name = "Phone Number")]
        [Required]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Please use the Format: 1234567890")]
        public long PhoneNumber { get; set; }
    }
}
