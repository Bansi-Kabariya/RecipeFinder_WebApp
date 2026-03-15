using System.ComponentModel.DataAnnotations;

public class User
{
    [Key]
    public int UserId { get; set; }

    // Username
    [Required(ErrorMessage = "Username is required")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 50 characters")]
    public string Username { get; set; }

    // Full Name
    [Required(ErrorMessage = "Full name is required")]
    [StringLength(100, ErrorMessage = "Full name cannot exceed 100 characters")]
    public string FullName { get; set; }

    // Email
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string Email { get; set; }

    // Password
    [Required(ErrorMessage = "Password is required")]
    [DataType(DataType.Password)]
    [StringLength(100, MinimumLength = 8,
        ErrorMessage = "Password must be at least 8 characters")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&]).{8,}$",
        ErrorMessage = "Password must contain uppercase, lowercase, number and special character")]
    public string Password { get; set; }

    // Confirm Password
    [Required(ErrorMessage = "Confirm password is required")]
    [Compare("Password", ErrorMessage = "Password and Confirm Password must match")]
    [DataType(DataType.Password)]
    public string ConfirmPassword { get; set; }

    // Mobile Number
    [Required(ErrorMessage = "Mobile number is required")]
    [Phone(ErrorMessage = "Invalid phone number")]
    [RegularExpression(@"^[6-9]\d{9}$", ErrorMessage = "Enter valid 10 digit mobile number")]
    public string MobileNumber { get; set; }

    // Address
    [Required(ErrorMessage = "Address is required")]
    [StringLength(250, ErrorMessage = "Address cannot exceed 250 characters")]
    public string Address { get; set; }

    // Gender
    [Required(ErrorMessage = "Gender is required")]
    public string Gender { get; set; }

    // Profile Picture
    public string ProfilePicture { get; set; }

    // Date of Birth
    [DataType(DataType.Date)]
    public DateTime? DateOfBirth { get; set; }

    // Role
    public string Role { get; set; } = "User";

    // Account status
    public bool IsActive { get; set; } = true;

    // Registration date
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}