namespace MockaBear.Models
{
    public class Client
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Image { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public string Phone { get; set; }
        public string Adress { get; set; }
        public DateTime DateAdded { get; set; } = DateTime.Now;

        public List<Order> Orders { get; set; }
        public List<Review> Reviews { get; set; }
    }
}
