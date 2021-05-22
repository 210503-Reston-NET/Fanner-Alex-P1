using DSModels;
using System.Collections.Generic;

namespace DSBL
{
    public interface IBuyerBL
    {
        DogBuyer AddBuyer(DogBuyer user);

        DogBuyer FindUser(long phone);

        List<DogBuyer> GetAllBuyers();

        List<DogBuyer> FindUserByName(string name);
    }
}