namespace DSModels
{
    /// <summary>
    /// Class allowing store to keep a dog as well as the quantity in the store.
    /// </summary>
    public abstract class Item
    {
        /// <summary>
        /// Basic constructor to initalize an item
        /// </summary>
        /// <param name="dog"> dog to be represented by the item</param>
        /// <param name="quant"> int representing the quantity of the dog at the store</param>
        public Item(Dog dog, int quant)
        {
            this.Dog = dog;
            this.Quantity = quant;
        }
        public Item() { }
        /// <summary>
        /// Dog to be represented by the item instance
        /// </summary>
        /// <value></value>
        public Dog Dog { get; set; }

        public int DogId { get; set; }

        /// <summary>
        /// Integer representing the number of dogs at the store
        /// </summary>
        /// <value></value>
        public int Quantity { get; set; }

        public override string ToString()
        {
            return "Dog: " + this.Dog + " Quantity: " + this.Quantity;
        }

        public bool Equals(Dog d)
        {
            return this.Dog.Equals(d);
        }

        public int Id { get; set; }
    }
}