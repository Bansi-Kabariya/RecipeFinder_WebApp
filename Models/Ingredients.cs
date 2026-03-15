using System.ComponentModel.DataAnnotations;

public class Ingredients
{
    [Key]
    public int IngredientId { get; set; }

    [Required]
    [StringLength(100)]
    public string IngredientName { get; set; }

    [Required]
    public string Quantity { get; set; }

    [Required]
    public int RecipeId { get; set; }
}