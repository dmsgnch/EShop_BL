using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SharedLibrary.Models.ClientDtoModels.MainModels;

public class OrderItemCDTO
{
    [Key]
    public Guid OrderItemCDtoId { get; set; } = Guid.NewGuid();
    public uint Quantity { get; set; }

    public decimal SummaryItemPrice
    {
        get
        {
            if (ProductCDto is null) return 0;
            return ProductCDto.PricePerUnit * Quantity;
        }
    }

    public OrderItemCDTO(Guid orderDtoId, Guid productDtoId, uint quantity = 1)
    {
        OrderCDtoId = orderDtoId;
        ProductCDtoId = productDtoId;

        Quantity = quantity;
    }

    #region Relationships

    //Order
    public Guid OrderCDtoId { get; set; }
    public OrderCDTO? OrderCDto { get; set; }

    //Product
    public Guid ProductCDtoId { get; set; }
    public ProductCDTO? ProductCDto { get; set; }
    
    #endregion
}