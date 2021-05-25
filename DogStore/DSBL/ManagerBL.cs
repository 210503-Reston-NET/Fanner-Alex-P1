using DSDL;
using DSModels;
using System.Collections.Generic;


namespace DSBL
{
    public class ManagerBL : IManagerBL
    {
        private Repo _repoDS;

        public ManagerBL(FannerDogsDBContext context)
        {
            _repoDS = new Repo(context);
        }

        public DogManager AddManager(DogManager user)
        {
            return _repoDS.AddManager(user);
        }

        public DogManager FindManager(long phone)
        {
            return _repoDS.FindManager(phone);
        }

        public List<DogManager> GetAllManagers()
        {
            return _repoDS.GetAllDogManagers();
        }

        public List<StoreLocation> GetManagerStores(long phone)
        {
            return _repoDS.GetManagerStores(phone);
        }
    }
}