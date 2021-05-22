namespace DSModels
{
    public class ManagesStore
    {
        public int Id { get; set; }
        public long DogManagerId { get; set; }
        public int StoreLocationId { get; set; }
        public DogManager Manager { get; set; }
        public StoreLocation Store { get; set; }
    }
}