using RecipeFinder_WebApp.Models;
using System.ComponentModel.DataAnnotations;

namespace RecipeFinder_WebApp.Models;

public class Reviews
{
    [Key]
    public int ReviewId { get; set; }

    // Foreign Keys
    public int RecipeId { get; set; }
    public int UserId { get; set; }

    // Navigation Properties (These are what we 'Include')
    public virtual Recipes Recipe { get; set; }
    public virtual User User { get; set; }

    public int Rating { get; set; }
    public string Comment { get; set; }
}