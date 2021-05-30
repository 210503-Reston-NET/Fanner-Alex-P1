using DSModels;
using System.Collections.Generic;

namespace DSBL
{
    public interface IOrderBL
    {
        List<OrderItem> GetOrderItems(int id);
        DogOrder UpdateOrder(int id);
        DogOrder AddOrder(DogOrder dogOrder);

        List<DogOrder> FindUserOrders(long phoneNumber, int option);

        List<DogOrder> FindStoreOrders(string address, string location, int option);
        DogOrder GetOrder(int id);
    }
}