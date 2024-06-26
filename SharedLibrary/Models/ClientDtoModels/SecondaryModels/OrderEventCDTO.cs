using System.ComponentModel.DataAnnotations;
using SharedLibrary.Models.ClientDtoModels.MainModels;
using SharedLibrary.Models.Enums;

namespace SharedLibrary.Models.ClientDtoModels.SecondaryModels;

public class OrderEventCDTO
{
    [Key] public Guid OrderEventCDtoId { get; set; } = Guid.NewGuid();
    
    public DateTime EventTime { get; set; }
    
    public OrderProcessingStage Stage { get; set; }

    public OrderEventCDTO(Guid orderId, OrderProcessingStage newStage = OrderProcessingStage.Cart)
    {
        OrderCDtoId = orderId;

        EventTime = DateTime.Now;
        Stage = newStage;
    }

    #region Relationships

    public Guid OrderCDtoId { get; set; }
    public OrderCDTO? OrderCDto { get; set; }

    #endregion
    
}