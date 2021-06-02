using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DSModels;
namespace DSWebUI.Models
{
    public class StoreLocationVM
    {
        public StoreLocationVM(long id)
        {
            CurrentManager = id;
        }
        public StoreLocationVM() { }
        public StoreLocationVM(StoreLocation sl)
        {
            Id = sl.Id;
            Address = sl.Address;
            Location = sl.Location;
        }
        public int Id { get; set; }

        [Display(Name = "Manager Number")]      
        public long CurrentManager { get; set; }
        /// <summary>
        /// String representing the address of the store.
        /// </summary>
        /// <value></value>
        [Display(Name = "Address")]
        
        [RegularExpression(@"^[\w\s]+,\s\w{2}$", ErrorMessage = "Please use the Format: City Name, ST")]
        public string Address { get; set; }

        /// <summary>
        /// String representing the location of the store.
        /// </summary>
        /// <value></value>
        [Display(Name = "Location Name")]
        public string Location { get; set; }
       
        /// <summary>
        /// Overriding the ToString() method to return basic information of the store.
        /// </summary>
        /// <returns>string representing the store's information</returns>
        public override string ToString()
        {
            return "Address: " + this.Address + " Location: " + this.Location;
        }

        
    }
}

