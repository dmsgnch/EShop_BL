using SharedLibrary.Models.ClientDtoModels.SecondaryModels;
using SharedLibrary.Models.Enums;

namespace SharedLibrary.Models.ClientDtoModels.MainModels;

public class OrderCDTO
{
    public Guid OrderCDtoId { get; set; } = Guid.NewGuid();

    public OrderProcessingStage? ProcessingStage
    {
        get
        {
            if (AnonymousToken is not null) return OrderProcessingStage.Cart;
            
            if (OrderEventsCDto is null || OrderEventsCDto.Count.Equals(0))
            {
                return null;
            }

            return OrderEventsCDto.MaxBy(oe => oe.EventTime)?.Stage;
        }
    }

    public Guid? AnonymousToken { get; set; }

    public decimal SummaryPrice
    {
        get
        {
            if (OrderItemsCDto is null) return 0;

            return OrderItemsCDto.Sum(oi => oi.SummaryItemPrice);
        }
    }

    public OrderCDTO(Guid? userCDtoId = null, Guid? anonymousToken = null)
    {
        if (userCDtoId is not null && anonymousToken is not null ||
            userCDtoId is null && anonymousToken is null)
        {
            throw new Exception("Order must have only user id or anonymous token!");
        }

        if (userCDtoId is not null)
        {
            UserCDtoId = userCDtoId;
        }
        else
        {
            AnonymousToken = anonymousToken;
        }
        //OrderEventsCDto.Add(new OrderEventCDTO(this.OrderCDtoId));
    }

    #region Relationships

    //User
    public Guid? UserCDtoId { get; set; }
    public UserCDTO? UserCDto { get; set; }

    //OrderEvent
    public List<OrderEventCDTO>? OrderEventsCDto { get; set; }

    //CartItem
    public List<OrderItemCDTO>? OrderItemsCDto { get; set; }

    //Recipient
    public RecipientCDTO? RecipientCDto { get; set; }

    //DeliveryAddress
    public DeliveryAddressCDTO? DeliveryAddressCDto { get; set; }

    #endregion
}