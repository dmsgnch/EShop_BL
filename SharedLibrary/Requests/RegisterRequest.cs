using System.ComponentModel.DataAnnotations;

namespace SharedLibrary.Requests;

[Serializable]
public class RegisterRequest
{
    [Required] public string Name { get; set; }
    [Required] public string LastName { get; set; }
    public string Patronymic { get; set; }

    [Required, EmailAddress] public string Email { get; set; }
    [Required, Phone] public string PhoneNumber { get; set; }

    [Required, DataType(DataType.Password)]
    public string Password { get; set; }
}