namespace MockaBear.Models
{
    public class CustomOrder
    {
        public int Id { get; set; }
        public string CustomeName { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;

        public string Status { get; set; } = "Pending";

        public int ClientId { get; set; }
        public Client Client { get; set; }

        public List<CustomOrder_Ingredient>? Custom_Ingredients { get; set; }
    }
}
