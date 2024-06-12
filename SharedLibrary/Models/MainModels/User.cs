using System.ComponentModel.DataAnnotations;
using SharedLibrary.Models.SecondaryModels;

namespace SharedLibrary.Models.MainModels;

public class User
{
    [Key]
    public Guid UserId { get; set; } = Guid.NewGuid();

    public string Name { get; set; }
    public string LastName { get; set; }
    public string? Patronymic { get; set; }
    
    public string PasswordHash { get; set; }
    public string Salt { get; set; }
    
    public string Email { get; set; }
    public string PhoneNumber { get; set; }

    #region Constructors
    
    public User() 
    {}
    
    public User(
        string name, 
        string lastName, 
        string passwordHash,
        string salt,
        string phoneNumber, 
        string email, 
        Role? role = null, 
        string? patronymic = null,
        string? anonymousToken = null)
    {
        RoleId = role?.RoleId ?? 1;
        Role = role;
        
        Name = name;
        LastName = lastName;
        Patronymic = patronymic;

        PasswordHash = passwordHash;
        Salt = salt;
        
        Email = email;
        PhoneNumber = phoneNumber;

        if (anonymousToken is not null)
        {
            Guid.TryParse(anonymousToken, out Guid result);
        
            UserId = result.Equals(Guid.Empty) ? 
                throw new ArgumentException($"Anonymous Token: {anonymousToken}, parsed incorrect") : 
                result;
        }
    }
    
    #endregion

    #region Relationships

    //Role
    public int RoleId { get; set; }
    public Role? Role { get; set; }
    
    //Order
    public List<Order> Orders { get; set; } = new();

    //Recipient
    public Recipient? Recipient { get; set; }

    //DeliveryAddress
    public DeliveryAddress? DeliveryAddress { get; set; }

    #endregion
}