using System.ComponentModel.DataAnnotations;

public class Categories
{
    [Key]
    public int CategoriesId { get; set; }

    [Required(ErrorMessage = "Categories name is required")]
    [StringLength(50)]
    public string CategoriesName { get; set; }

    [StringLength(250)]
    public string Description { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
}