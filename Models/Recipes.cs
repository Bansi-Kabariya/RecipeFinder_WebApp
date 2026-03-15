using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecipeFinder_WebApp.Models // Added namespace
{
    public class Recipes
    {
        [Key]
        public int RecipeId { get; set; }

        [Required]
        [StringLength(150)]
        public string? RecipeName { get; set; } // Added ? to fix warnings

        [Required]
        public int CategoriesId { get; set; }

        [ForeignKey("CategoriesId")]
        public virtual Categories? Categories { get; set; }

        [Required]
        [StringLength(1000)]
        public string? Description { get; set; }

        [Required]
        public string? Instructions { get; set; }

        [Range(1, 5000)]
        public int Calories { get; set; }

        [Range(1, 600)]
        public int CookingTime { get; set; }

        [Range(1, 20)]
        public int Servings { get; set; }

        public string? ImageUrl { get; set; }
        public string? ImagePath { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}