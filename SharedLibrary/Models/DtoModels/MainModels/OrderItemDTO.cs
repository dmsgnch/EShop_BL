using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SharedLibrary.Models.DtoModels.MainModels;

public class OrderItemDTO
{
    [Key]
    public Guid OrderItemDtoId { get; set; } = Guid.NewGuid();
    public uint Quantity { get; set; }
    
    #region Constructors

    public OrderItemDTO(Guid orderDtoId, Guid productDtoId, uint quantity = 1)
    {
        OrderDtoId = orderDtoId;
        ProductDtoId = productDtoId;

        Quantity = quantity;
    }
    
    #endregion

    #region Relationships

    //Order
    public Guid OrderDtoId { get; set; }
    public OrderDTO? OrderDto { get; set; }

    //Product
    public Guid ProductDtoId { get; set; }
    public ProductDTO? ProductDto { get; set; }
    
    #endregion
}