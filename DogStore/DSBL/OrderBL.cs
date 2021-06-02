using DSDL;
using DSModels;
using System.Collections.Generic;


namespace DSBL
{
    public class OrderBL : IOrderBL
    {
        private Repo _repoDS;

        public OrderBL(FannerDogsDBContext context)
        {
            _repoDS = new Repo(context);
        }

        public DogOrder AddOrder(DogOrder dogOrder)
        {
            return _repoDS.AddOrder(dogOrder);
        }
        /// <summary>
        /// Check if quantity is valid and then insert into database
        /// </summary>
        /// <param name="orderItem">Order Item to be added</param>
        /// <param name="maxquant">Max quantity user can buy</param>
        /// <param name="storeId">Store which items are to be ordered from</param>
        /// <returns>Order Item added, null otherwise</returns>
        public OrderItem AddOrderItem(OrderItem orderItem, int maxquant, int storeId)
        {
            if (orderItem.Quantity > maxquant) return null;
            else if (orderItem.Quantity == 0) return null;
            else return _repoDS.AddOrderItem(orderItem, storeId);
        }

        public List<DogOrder> FindStoreOrders(string address, string location, int option)
        {
            return _repoDS.FindStoreOrders(address, location, option);
        }

        public List<DogOrder> FindUserOrders(long phoneNumber, int option)
        {
            return _repoDS.FindUserOrders(phoneNumber, option);
        }

        public DogOrder GetOrder(int id)
        {
            return _repoDS.GetOrder(id);
        }

        public List<OrderItem> GetOrderItems(int id) {
            return _repoDS.GetOrderItems(id);
        }
        public DogOrder UpdateOrder(DogOrder dogOrder)
        {
            return _repoDS.UpdateOrder(dogOrder);
        }
    }
}