using System.ComponentModel.DataAnnotations;

public class Save
{
    [Key]
    public int FolderId { get; set; }

    [Required]
    [StringLength(100)]
    public string FolderName { get; set; }

    public int UserId { get; set; }
}