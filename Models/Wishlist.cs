using System.ComponentModel.DataAnnotations;

public class Wishlist
{
    [Key]
    public int WishlistId { get; set; }

    public int UserId { get; set; }

    public int RecipeId { get; set; }

    public DateTime AddedAt { get; set; } = DateTime.Now;
}