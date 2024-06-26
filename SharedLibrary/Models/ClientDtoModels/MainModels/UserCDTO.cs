using SharedLibrary.Models.ClientDtoModels.SecondaryModels;

namespace SharedLibrary.Models.ClientDtoModels.MainModels;

public class UserCDTO
{
    public Guid UserCDtoId { get; set; } = Guid.NewGuid();

    public string Name { get; set; }
    public string LastName { get; set; }
    public string? Patronymic { get; set; }
    public string? Password { get; set; }
    
    public string Email { get; set; }
    public string PhoneNumber { get; set; }

    public UserCDTO()
    { }
    
    public UserCDTO(
        string name, 
        string lastName,
        string phoneNumber, 
        string email, 
        int roleId, 
        string? patronymic = null,
        string? password = null)
    {
        RoleCDtoId = roleId;
        
        Name = name;
        LastName = lastName;
        Patronymic = patronymic;

        Password = password;
        
        Email = email;
        PhoneNumber = phoneNumber;
    }

    #region Relationships

    //Role
    public int RoleCDtoId { get; set; } = 1;
    public RoleCDTO? RoleCDto { get; set; }
    
    //Seller
    public Guid? SellerCDtoId { get; set; }
    public SellerCDTO? SellerCDto { get; set; }
    
    //Order
    public List<OrderCDTO>? OrdersCDto { get; set; }

    //Recipient
    public RecipientCDTO? RecipientCDto { get; set; }

    //DeliveryAddress
    public DeliveryAddressCDTO? DeliveryAddressCDto { get; set; }

    #endregion
}