using System.ComponentModel.DataAnnotations;
using SharedLibrary.Models.ClientDtoModels.MainModels;

namespace SharedLibrary.Models.ClientDtoModels.SecondaryModels;

public class RecipientCDTO
{
    [Key] public Guid RecipientCDtoId { get; set; }

    public string Name { get; set; } = "";
    public string LastName { get; set; } = "";
    public string? Patronymic { get; set; } = null;

    public string PhoneNumber { get; set; } = "";
    
    public RecipientCDTO(
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

        UserCDtoId = userId;
        OrderCDtoId = orderId;
    }

    #region Relationships

    //User
    public Guid? UserCDtoId { get; set; }
    public UserCDTO? UserCDto { get; set; }

    //Order
    public Guid? OrderCDtoId { get; set; }
    public OrderCDTO? OrderCDto { get; set; }
    
    #endregion
}