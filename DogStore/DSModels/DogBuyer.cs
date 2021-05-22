namespace DSModels
{
    /// <summary>
    /// Class for each customer, implements UserInterface
    /// </summary>
    public class DogBuyer : UserInterface
    {
        public DogBuyer()
        {
        }

        public DogBuyer(string name, string address, long phoneNumber)
        {
            this.Name = name;
            this.Address = address;
            this.PhoneNumber = phoneNumber;
        }

        /// <summary>
        /// String representing the customer's name.
        /// </summary>
        /// <value></value>
        public string Name { get; set; }

        public string Address { get; set; }
        public long PhoneNumber { get; set; }

        /// <summary>
        /// Overriding ToString method to return string representing customer
        /// </summary>
        /// <returns>Customer's name</returns>
        public override string ToString()
        {
            return this.Name;
        }
    }
}