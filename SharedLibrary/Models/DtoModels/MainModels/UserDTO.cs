using SharedLibrary.Models.DtoModels.SecondaryModels;

namespace SharedLibrary.Models.DtoModels.MainModels;

public class UserDTO
{
    public Guid UserDtoId { get; set; } = Guid.NewGuid();

    public string Name { get; set; }
    public string LastName { get; set; }
    public string? Patronymic { get; set; }
    
    public string PasswordHash { get; set; }
    public string Salt { get; set; }
    
    public string Email { get; set; }
    public string PhoneNumber { get; set; }

    #region Constructors
    
    public UserDTO(
        string name, 
        string lastName, 
        string passwordHash,
        string salt,
        string phoneNumber, 
        string email, 
        RoleDTO? role = null, 
        string? patronymic = null,
        string? anonymousToken = null)
    {
        RoleDtoId = role?.RoleDtoId ?? 1;
        RoleDto = role;
        
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
        
            UserDtoId = result.Equals(Guid.Empty) ? 
                throw new ArgumentException($"Anonymous Token: {anonymousToken}, parsed incorrect") : 
                result;
        }
    }
    
    #endregion

    #region Relationships

    //Role
    public int RoleDtoId { get; set; } = 1;
    public RoleDTO? RoleDto { get; set; }
    
    //Seller
    public Guid? SellerDtoId { get; set; }
    public SellerDTO? SellerDto { get; set; }
    
    //Order
    public List<OrderDTO> OrdersDto { get; set; } = new();

    //Recipient
    public RecipientDTO? RecipientDto { get; set; }

    //DeliveryAddress
    public DeliveryAddressDTO? DeliveryAddressDto { get; set; }

    #endregion
}