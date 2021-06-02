using DSModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DSWebUI.Models
{
    public class BuyerHistoryVM
    {
        public BuyerHistoryVM() { }
        [Required]
        [Display(Name = "Phone Number")]
        public long BuyerNumber { get; set; }
        [Display(Name = "How Would You Like The Orders Displayed?")]
        [Required]
        public int OrderOption { get; set; }
        public int OrderId { get; set; }
        public int StoreId { get; set; }
        [Display(Name = "User Phone Number")]
        public long BuyerId { get; set; }
        [Display(Name = "Buyer Name")]
        public string BuyerName { get; set; }
        [Display(Name = "Store Name")]
        public string StoreName { get; set; }
        [Display(Name = "Store Address")]
        public string Address { get; set; }
        [Display(Name = "Date Ordered")]
        public DateTime OrderDate { get; set; }
        public DogBuyer DogBuyer { get; set; }
        public StoreLocation StoreLocation { get; set; }
        [Display(Name = "Order Total")]
        public double Total { get; set; }
    }
}
