namespace MockaBear.Models
{
    public class Client
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public string Phone { get; set; }
        public string Adress { get; set; }

        public List<Order> Orders { get; set; }
        public List<Review> Reviews { get; set; }
    }
}
