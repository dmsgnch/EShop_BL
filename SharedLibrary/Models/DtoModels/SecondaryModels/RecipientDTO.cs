using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SharedLibrary.Models.DtoModels.MainModels;

namespace SharedLibrary.Models.DtoModels.SecondaryModels;

public class RecipientDTO
{
    [Key] public Guid RecipientDtoId { get; set; }

    public string Name { get; set; } = "";
    public string LastName { get; set; } = "";
    public string? Patronymic { get; set; } = null;

    public string PhoneNumber { get; set; } = "";
    
    public RecipientDTO(
        string name, 
        string lastName, 
        string phoneNumber, 
        string? patronymic = null,
        UserDTO? user = null,
        OrderDTO? order = null)
    {
        Name = name;
        LastName = lastName;
        Patronymic = patronymic;
        
        PhoneNumber = phoneNumber;
        
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

    #region Relationships

    //User
    public Guid? UserDtoId { get; set; }
    public UserDTO? UserDto { get; set; }

    //Order
    public Guid? OrderDtoId { get; set; }
    public OrderDTO? OrderDto { get; set; }
    
    #endregion
}