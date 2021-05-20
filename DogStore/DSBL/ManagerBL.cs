using System.Collections.Generic;
using DSModels;
using DSDL;
using Entity = DSDL.Entities;
namespace DSBL
{
    public class ManagerBL : IManagerBL
    {
        private Repo _repoDS;
        public ManagerBL(Entity.FannerDogsDBContext context ){
            _repoDS =  new Repo(context);
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
    }
}