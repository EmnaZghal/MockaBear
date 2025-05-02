namespace MockaBear.Models
{
    public class Cart
    {
        public int Id { get; set; }

        public int? ClientId { get; set; }
        public Client? Client { get; set; }

        public decimal TotalAmount { get; set; }
        public bool IsCheckedOut { get; set; }
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        /* navigation */
        public List<CartItem> Items { get; set; } = new();
    }
}
