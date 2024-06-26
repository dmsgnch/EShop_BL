using SharedLibrary.Models.DtoModels.SecondaryModels;

namespace SharedLibrary.Models.DtoModels.MainModels;

public class UserDTO
{
    public Guid UserDtoId { get; set; } = Guid.NewGuid();

    public string Name { get; set; }
    public string LastName { get; set; }
    public string? Patronymic { get; set; }
    
    public string? PasswordHash { get; set; }
    public string? Salt { get; set; }
    
    public string Email { get; set; }
    public string PhoneNumber { get; set; }

    public UserDTO(
        string name, 
        string lastName,
        string phoneNumber, 
        string email,
        string? patronymic = null,
        string? passwordHash = null,
        string? salt = null)
    {
        Name = name;
        LastName = lastName;
        Patronymic = patronymic;

        PasswordHash = passwordHash;
        Salt = salt;
        
        Email = email;
        PhoneNumber = phoneNumber;
    }

    #region Relationships

    //Role
    public int RoleDtoId { get; set; } = 1;
    public RoleDTO? RoleDto { get; set; }
    
    //Seller
    public Guid? SellerDtoId { get; set; }
    public SellerDTO? SellerDto { get; set; }
    
    //Order
    public List<OrderDTO>? OrdersDto { get; set; }

    //Recipient
    public RecipientDTO? RecipientDto { get; set; }

    //DeliveryAddress
    public DeliveryAddressDTO? DeliveryAddressDto { get; set; }

    #endregion
}