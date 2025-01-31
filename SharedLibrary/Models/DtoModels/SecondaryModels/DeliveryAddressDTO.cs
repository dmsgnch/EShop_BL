using SharedLibrary.Models.DtoModels.MainModels;

namespace SharedLibrary.Models.DtoModels.SecondaryModels;

public class DeliveryAddressDTO
{
    public Guid DeliveryAddressDtoId { get; set; }

    public string City { get; set; }
    public string Street { get; set; }
    public string House { get; set; }
    public string? Apartment { get; set; }
    public string? Floor { get; set; }

    #region Constructors

    public DeliveryAddressDTO(
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

        UserDtoId = userDtoId;
        OrderDtoId = orderDtoId;
    }
        
    #endregion
        
    #region Relationships

    //User
    public Guid? UserDtoId { get; set; }
    public UserDTO? UserDto { get; set; }

    //Order
    public Guid? OrderDtoId { get; set; }
    public OrderDTO? OrderDto { get; set; }
    
    #endregion
}