using DSModels;
using System.Collections.Generic;

namespace DSBL
{
    public interface IOrderBL
    {
        List<OrderItem> GetOrderItems(int id);
        DogOrder UpdateOrder(DogOrder dogOrder);
        DogOrder AddOrder(DogOrder dogOrder);

        List<DogOrder> FindUserOrders(long phoneNumber, int option);

        List<DogOrder> FindStoreOrders(string address, string location, int option);
        DogOrder GetOrder(int id);
        OrderItem AddOrderItem(OrderItem orderItem, int maxquant, int storeId);
    }
}