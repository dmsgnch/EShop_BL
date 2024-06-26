using SharedLibrary.Models.DtoModels.SecondaryModels;
using SharedLibrary.Models.Enums;

namespace SharedLibrary.Models.DtoModels.MainModels;

public class OrderDTO
{
    public Guid OrderDtoId { get; set; } = Guid.NewGuid();

    public OrderProcessingStage? ProcessingStage
    {
        get
        {
            if (AnonymousToken is not null) return OrderProcessingStage.Cart;
            if (OrderEventsDto is null || OrderEventsDto.Count.Equals(0))
            {
                return null;
            }

            return OrderEventsDto.MaxBy(oe => oe.EventTime)?.Stage;
        }
    } 
    public Guid? AnonymousToken { get; set; }

    public OrderDTO()
    { }

    public OrderDTO(Guid? userDtoId = null, Guid? anonymousToken = null)
    {
        if (userDtoId is not null && anonymousToken is not null ||
            userDtoId is null && anonymousToken is null)
        {
            throw new Exception("Order must have only user id or anonymous token!");
        }
        
        if (userDtoId is not null)
        {
            UserDtoId = userDtoId;
        }
        else
        {
            AnonymousToken = anonymousToken;
        }
        
        //OrderEventsDto.Add(new OrderEventDTO(OrderDtoId));
    }

    #region Relationships

    //User
    public Guid? UserDtoId { get; set; }
    public UserDTO? UserDto { get; set; }
    
    //OrderEvent
    public List<OrderEventDTO>? OrderEventsDto { get; set; }

    //CartItem
    public List<OrderItemDTO>? OrderItemsDto { get; set; }

    //Recipient
    public RecipientDTO? RecipientDto { get; set; }

    //DeliveryAddress
    public DeliveryAddressDTO? DeliveryAddressDto { get; set; }

    #endregion
}