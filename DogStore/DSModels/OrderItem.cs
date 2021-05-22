namespace DSModels
{
    public class OrderItem : Item
    {
        public OrderItem()
        {
        }

        public OrderItem(Dog dog, int quantity) : base(dog, quantity)
        {
        }

        public DogOrder AssociatedOrder { get; set; }
        public int OrderId { get; set; }
    }
}