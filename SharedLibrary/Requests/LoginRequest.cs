using System.ComponentModel.DataAnnotations;

namespace SharedLibrary.Requests;

[Serializable]
public class LoginRequest
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid Email Address")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [DataType(DataType.Password)]
    [StringLength(30, MinimumLength = 6, ErrorMessage = "Password must be between 2 and 50 characters")]
    public string Password { get; set; }
}