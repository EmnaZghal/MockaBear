namespace MockaBear.Models
{
    public class CustomOrder_Ingredient
    {
        public int Id { get; set; }
        public int CustomOrderId { get; set; }
        public CustomOrder CustomOrder { get; set; }

        public int IngredientId { get; set; }
        public Ingredient Ingredient { get; set; }

        // Exemple de propriété additionnelle
        public int Quantity { get; set; }
    }
}
