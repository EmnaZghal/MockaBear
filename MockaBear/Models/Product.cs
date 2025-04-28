namespace MockaBear.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }

        public bool IsAvailable { get; set; }
        public DateTime DateAdded { get; set; } = DateTime.Now;
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public List<Review>? Reviews { get; set; }
    }
}
