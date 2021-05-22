using DSBL;
using System;

namespace DSUI
{
    public class GeneralMenu : IMenu
    {
        private IStoreLocationBL _storeLoBL;
        private IBuyerBL _buyerBL;
        private IOrderBL _orderBL;
        private IManagerBL _managerBL;

        public GeneralMenu(IStoreLocationBL StoreLoBL, IBuyerBL buyerBL, IOrderBL orderBL, IManagerBL managerBL)
        {
            this._storeLoBL = StoreLoBL;
            this._buyerBL = buyerBL;
            this._orderBL = orderBL;
            this._managerBL = managerBL;
        }

        /// <summary>
        /// Method that opens starting state of the menu
        /// </summary>
        public void OnStart()
        {
            bool repeat = true;
            do
            {
                Console.WriteLine("Welcome to the Fanner Furry Friends Dog Ordering Service!");
                Console.WriteLine("Enter a letter corresponding to your user status");
                Console.WriteLine("[a] A customer");
                Console.WriteLine("[b] A manager");

                string input = Console.ReadLine();
                switch (input)
                {
                    case "0":
                        ViewStoreList();
                        break;

                    case "1":
                        ViewStoreInv();
                        break;

                    case "2":
                        OrderDog();
                        break;

                    case "7":
                        //StoreLocation storeLocation = _storeLoBL.AddStoreLocation(new StoreLocation("test", "here"));
                        break;

                    case "a":
                        IMenu _custMenu = new CustomerMenu(_storeLoBL, _buyerBL, _orderBL);
                        _custMenu.OnStart();
                        break;

                    case "b":
                        IMenu _managerMenu = new ManagerMenu(_storeLoBL, _orderBL, _managerBL);
                        _managerMenu.OnStart();
                        break;

                    default:
                        repeat = false;
                        break;
                }
            } while (repeat);
        }

        private void ViewStoreList()
        {
        }

        private void ViewStoreInv()
        {
        }

        private void OrderDog()
        {
        }
    }
}