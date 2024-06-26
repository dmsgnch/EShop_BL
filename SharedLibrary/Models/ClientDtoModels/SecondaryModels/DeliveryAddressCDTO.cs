using SharedLibrary.Models.ClientDtoModels.MainModels;

namespace SharedLibrary.Models.ClientDtoModels.SecondaryModels;

public class DeliveryAddressCDTO
{
    public Guid DeliveryAddressCDtoId { get; set; }

    public string City { get; set; }
    public string Street { get; set; }
    public string House { get; set; }
    public string? Apartment { get; set; }
    public string? Floor { get; set; }

    #region Constructors

    public DeliveryAddressCDTO(
        string city, 
        string street, 
        string house, 
        string? apartment = null, 
        string? floor = null,
        Guid? userDtoId = null,
        Guid? orderDtoId = null
    )
    {
        City = city;
        Street = street;
        House = house;
        Apartment = apartment;
        Floor = floor;

        if (userDtoId is null && orderDtoId is null)
        {
            throw new ArgumentException("You must pass the Order or User");
        }

        if (userDtoId is not null && orderDtoId is not null)
        {
            throw new ArgumentException("You cant pass Order and User at the same time");
        }

        UserCDtoId = userDtoId;
        OrderCDtoId = orderDtoId;
    }
        
    #endregion
        
    #region Relationships

    //User
    public Guid? UserCDtoId { get; set; }
    public UserCDTO? UserCDto { get; set; }

    //Order
    public Guid? OrderCDtoId { get; set; }
    public OrderCDTO? OrderCDto { get; set; }
    
    #endregion
}