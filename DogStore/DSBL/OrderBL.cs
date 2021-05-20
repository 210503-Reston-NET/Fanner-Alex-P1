using System.Collections.Generic;
using DSDL;
using DSModels;
using Entity = DSDL.Entities;
namespace DSBL
{
    public class OrderBL : IOrderBL
    {

        private Repo _repoDS;
        public OrderBL(Entity.FannerDogsDBContext context ){
            _repoDS =  new Repo(context);
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