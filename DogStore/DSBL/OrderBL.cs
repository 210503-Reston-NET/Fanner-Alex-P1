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

        public List<DogOrder> FindStoreOrders(string address, string location, int option)
        {
            return _repoDS.FindStoreOrders(address, location, option);
        }

        public List<DogOrder> FindUserOrders(long phoneNumber, int option)
        {
            return _repoDS.FindUserOrders(phoneNumber, option);
        }
    }
}