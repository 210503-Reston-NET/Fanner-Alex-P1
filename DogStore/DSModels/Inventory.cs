namespace DSModels
{
    public class Inventory : Item
    {
        public Inventory()
        {
            
         
        }

        public Inventory(Dog dog, int quantity) : base(dog, quantity)
        {
        }

        public StoreLocation Store { get; set; }
        public int StoreId { get; set; }
    }
}