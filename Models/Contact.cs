using System.ComponentModel.DataAnnotations;

public class Contact
{
    [Key]
    public int ContactId { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [StringLength(1000)]
    public string Message { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
}