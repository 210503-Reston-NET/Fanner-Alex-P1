using DSModels;
using System.Collections.Generic;

namespace DSBL
{
    public interface IManagerBL
    {
        List<DogManager> GetAllManagers();

        DogManager AddManager(DogManager user);

        DogManager FindManager(long phone);
        List<StoreLocation> GetManagerStores(long phone);
    }
}