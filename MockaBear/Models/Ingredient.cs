namespace MockaBear.Models
{
    public class Ingredient
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }

        public int Number { get; set; }

        public List<CustomOrder> CustomOrders { get; set; }
    }
}
