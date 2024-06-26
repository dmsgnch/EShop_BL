using System.ComponentModel.DataAnnotations;
using SharedLibrary.Models.Enums;
using SharedLibrary.Models.DtoModels.MainModels;

namespace SharedLibrary.Models.DtoModels.SecondaryModels;

public class OrderEventDTO
{
    [Key]
    public Guid OrderEventDtoId { get; set; }
    
    public DateTime EventTime { get; set; }
    
    public OrderProcessingStage Stage { get; set; }

    public OrderEventDTO(Guid orderId, OrderProcessingStage newStage = OrderProcessingStage.Cart)
    {
        OrderDtoId = orderId;

        EventTime = DateTime.Now;
        Stage = newStage;
    }

    #region Relationships

    public Guid OrderDtoId { get; set; }
    public OrderDTO? OrderDto { get; set; }

    #endregion
    
}