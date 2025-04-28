﻿namespace MockaBear.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public decimal TotalAmount { get; set; }

        public string Status { get; set; }
        public int ClientId { get; set; }
        public Client Client { get; set; }

        public List<OrderItem>? OrderItems { get; set; }
    }
}
