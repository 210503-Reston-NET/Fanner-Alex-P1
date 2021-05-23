using DSDL;
using DSModels;
using System.Collections.Generic;


namespace DSBL
{
    public class BuyerBL : IBuyerBL
    {
        private Repo _repoDS;

        public BuyerBL(FannerDogsDBContext context)
        {
            _repoDS = new Repo(context);
        }

        public DogBuyer AddBuyer(DogBuyer user)
        {
            return _repoDS.AddBuyer(user);
        }

        public DogBuyer FindUser(long phone)
        {
            return _repoDS.FindBuyer(phone);
        }

        public List<DogBuyer> FindUserByName(string name)
        {
            return _repoDS.FindBuyerByName(name);
        }

        public List<DogBuyer> GetAllBuyers()
        {
            return _repoDS.GetAllBuyers();
        }
    }
}