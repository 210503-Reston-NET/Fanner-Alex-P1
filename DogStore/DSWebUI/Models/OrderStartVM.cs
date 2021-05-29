using DSModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DSWebUI.Models
{
    public class OrderStartVM
    {
        public OrderStartVM() { }
        public int StoreId { get; set; }
        public long BuyerId { get; set; }
        public int BuyPointer { get; set; }
        public int TotLength { get; set; }
        public DateTime OrderDate { get; set; }
        public string BuyerName { get; set; }

        public string BuyerAddress { get; set; }
        public long BuyerPhoneNumber { get; set; }

        public string StoreAddress { get; set; }
        public List<StoreLocation> StoreLocations{ get;set;}
        public List<DogBuyer> DogBuyers { get; set; }

        /// <summary>
        /// String representing the location of the store.
        /// </summary>
        /// <value></value>
        public string StoreName { get; set; }
        /// <summary>
        /// Customer ordering the dogs, represented by DogBuyer.
        /// </summary>
        /// <value></value>
        public DogBuyer DogBuyer { get; set; }

        /// <summary>
        /// StoreLocation that the customer is ordering from.
        /// </summary>
        /// <value></value>
        public StoreLocation StoreLocation { get; set; }
    }
}
