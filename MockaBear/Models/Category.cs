using System.ComponentModel.DataAnnotations;
namespace MockaBear.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "The category name is required.")]
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime DateAdded { get; set; } = DateTime.Now;

        public List<Product>? Products { get; set; }
    }
}
