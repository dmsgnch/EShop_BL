using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SharedLibrary.Models.DtoModels.MainModels;

namespace SharedLibrary.Models.DtoModels.SecondaryModels;

public class RecipientDTO
{
    [Key] public Guid RecipientDtoId { get; set; }

    public string Name { get; set; }
    public string LastName { get; set; }
    public string? Patronymic { get; set; }

    public string PhoneNumber { get; set; }

    public RecipientDTO(
        string name,
        string lastName,
        string phoneNumber,
        string? patronymic = null,
        Guid? userId = null,
        Guid? orderId = null)
    {
        Name = name;
        LastName = lastName;
        Patronymic = patronymic;

        PhoneNumber = phoneNumber;

        if (userId is null && orderId is null)
        {
            throw new ArgumentException("You must pass the Order or User");
        }

        if (userId is not null && orderId is not null)
        {
            throw new ArgumentException("You cant pass Order and User at the same time");
        }

        UserDtoId = userId;
        OrderDtoId = orderId;
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