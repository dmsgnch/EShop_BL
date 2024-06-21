using System.ComponentModel.DataAnnotations;
using SharedLibrary.Models.DbModels.MainModels;
using SharedLibrary.Models.Enums;
using SharedLibrary.Models.DtoModels.MainModels;

namespace SharedLibrary.Models.DbModels.SecondaryModels;

public class OrderEvent
{
    [Key]
    public Guid OrderEventId { get; set; }
    
    public DateTime EventTime { get; set; }
    
    public OrderProcessingStage Stage { get; set; }
    
    #region Constructors
    
    public OrderEvent() 
    {}

    public OrderEvent(Order order, OrderProcessingStage newStage = OrderProcessingStage.Cart)
    {
        Order = order;
        OrderId = order.OrderId;
        
        if (!newStage.Equals(OrderProcessingStage.Cart) && (int)Order.ProcessingStage <= (int)newStage)
        {
            throw new ArgumentException("New stages cannot precede or coincide with a previous stage!");
        }

        EventTime = DateTime.Now;
        Stage = newStage;
    }
    
    #endregion

    #region Relationships

    public Guid OrderId { get; set; }
    public Order? Order { get; set; }

    #endregion
    
}