using System;
using System.Collections.Generic;

namespace DSModels
{
    /// <summary>
    /// Class representing the order a customer places at a store.
    /// </summary>
    public class DogOrder
    {
        private List<Item> _itemsInOrder;
  
        public DogOrder()
        {
        }

        public DogOrder(DogBuyer buyer, double tot, StoreLocation sl)
        {
            this.DogBuyer = buyer;
            this.StoreLocation = sl;
            this.Total = tot;
            _itemsInOrder = new List<Item>();
            this.OrderDate = DateTime.Now;
        }

        public DogOrder(DogBuyer buyer, double tot, StoreLocation sl, int id) : this(buyer, tot, sl)
        {
            this.Id = id;
        }
        public int Id { get; set; }
        public int StoreId { get; set; }
        public long BuyerId { get; set; }
        public DateTime OrderDate { get; set; }

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

        /// <summary>
        /// Double representing the total of the order.
        /// </summary>
        /// <value></value>
        public double Total { get; set; }

        public Item AddItemToOrder(Item item)
        {
            _itemsInOrder.Add(item);
            return item;
        }

        public List<Item> GetItems()
        {
            return _itemsInOrder;
        }

        public override string ToString()
        {
            string s = this.DogBuyer + " ordered from "
            + this.StoreLocation + " at the following time: " +
            this.OrderDate.ToString() + ". The total was "
            + this.Total.ToString();
            s += " The following items were ordered: ";
            foreach (Item item in this.GetItems())
            {
                s += item.ToString() + " ";
            }
            return s;
        }
    }
}