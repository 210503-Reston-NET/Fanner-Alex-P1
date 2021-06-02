using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DSModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DSWebUI.Models
{
    public class DogOrderVM
    {
        public int Id { get; set; }
        [Display(Name = "Store Id")]
        public int StoreId { get; set; }
        [Display(Name = "Buyer Number")]
        public long BuyerId { get; set; }
        [Display(Name = "Order Date")]
        public DateTime OrderDate { get; set; }

        /// <summary>
        /// Customer ordering the dogs, represented by DogBuyer.
        /// </summary>
        /// <value></value>
        public DogBuyer DogBuyer { get; set; }
        public List<long> BuyerList { get; set; }

        /// <summary>
        /// StoreLocation that the customer is ordering from.
        /// </summary>
        /// <value></value>
        public StoreLocation StoreLocation { get; set; }
        
        /// <summary>
        /// Double representing the total of the order.
        /// </summary>
        /// <value></value>
        public double Total { get; set; }
    }
}
