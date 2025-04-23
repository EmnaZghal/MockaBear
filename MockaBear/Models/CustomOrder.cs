namespace MockaBear.Models
{
    public class CustomOrder
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public DateTime OrderDate { get; set; }

        public int ClientId { get; set; }
        public Client Client { get; set; }

        public List<Ingredient> Ingredients { get; set; }
    }
}
