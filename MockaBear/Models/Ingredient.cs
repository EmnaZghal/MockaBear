namespace MockaBear.Models
{
    public class Ingredient
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }

        public int Number { get; set; }
        public string Task { get; set; }
        public int TaskNumber { get; set; }
        public DateTime DateAdded { get; set; } = DateTime.Now;

        public List<CustomOrder_Ingredient>? CustomOrder_Ingredients { get; set; }
    }
}
