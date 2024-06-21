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
        UserDTO? user = null,
        OrderDTO? order = null
    )
    {
        City = city;
        Street = street;
        House = house;
        Apartment = apartment;
        Floor = floor;

        if (order is null && user is null)
        {
            throw new ArgumentException("You must pass the Order or User");
        }

        if (order is not null && user is not null)
        {
            throw new ArgumentException("You cant pass Order and User at the same time");
        }

        UserDtoId = user?.UserDtoId;
        UserDto = user;

        OrderDtoId = order?.OrderDtoId;
        OrderDto = order;
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