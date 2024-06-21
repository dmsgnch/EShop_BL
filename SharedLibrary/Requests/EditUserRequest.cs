using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace SharedLibrary.Requests;

[Serializable]
public class EditUserRequest
{
    public Guid UserId { get; set; } 
    
    [Required(ErrorMessage = "Name is required")]
    [DataType(DataType.Text)]
    [StringLength(30, MinimumLength = 2, ErrorMessage = "The name must be a minimum of 6 and a maximum of 30 characters")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Last name is required")]
    [DataType(DataType.Text)]
    [StringLength(30, MinimumLength = 2, ErrorMessage = "The last name must be a minimum of 6 and a maximum of 30 characters")]
    public string LastName { get; set; }
    
    [MaybeNull]
    [DataType(DataType.Text)]
    [StringLength(30, MinimumLength = 2, ErrorMessage = "The patronymic must be a minimum of 6 and a maximum of 30 characters")]
    public string? Patronymic { get; set; }
    
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid Email Address")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Phone number is required")]
    [Phone(ErrorMessage = "Invalid phone number")]
    public string PhoneNumber { get; set; }

    [MaybeNull]
    [StringLength(30, MinimumLength = 6,
        ErrorMessage = "The password must be a minimum of 6 and a maximum of 30 characters")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*[\W_]).+$",
        ErrorMessage =
            "Password must contain at least one uppercase letter, one lowercase letter, and one " +
            "special character.")]
    [DataType(DataType.Password)]
    public string? Password { get; set; }

    [MaybeNull]
    [StringLength(30, MinimumLength = 6,
        ErrorMessage = "The password must be a minimum of 6 and a maximum of 30 characters")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*[\W_]).+$",
        ErrorMessage =
            "Password must contain at least one uppercase letter, one lowercase letter, and one " +
            "special character.")]
    [Compare("Password", ErrorMessage = "The passwords don't match")]
    [DataType(DataType.Password)]
    public string? PasswordConfirm { get; set; }

    public int RoleId { get; set; } = 1;
    public Guid? SellerId { get; set; }
}