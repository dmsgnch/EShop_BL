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

    public OrderEventDTO(OrderDTO order, OrderProcessingStage newStage = OrderProcessingStage.Cart)
    {
        OrderDto = order;
        OrderDtoId = order.OrderDtoId;
        
        if (!newStage.Equals(OrderProcessingStage.Cart) && (int)OrderDto.ProcessingStage <= (int)newStage)
        {
            throw new ArgumentException("New stages cannot precede or coincide with a previous stage!");
        }

        EventTime = DateTime.Now;
        Stage = newStage;
    }

    #region Relationships

    public Guid OrderDtoId { get; set; }
    public OrderDTO? OrderDto { get; set; }

    #endregion
    
}