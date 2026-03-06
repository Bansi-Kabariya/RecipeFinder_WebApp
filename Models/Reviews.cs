using System.ComponentModel.DataAnnotations;

public class Reviews
{
    [Key]
    public int ReviewId { get; set; }

    [Required]
    public int UserId { get; set; }

    [Required]
    public int RecipeId { get; set; }

    [Required]
    [Range(1, 5)]
    public int Rating { get; set; }

    [Required]
    [StringLength(500)]
    public string Comment { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
}