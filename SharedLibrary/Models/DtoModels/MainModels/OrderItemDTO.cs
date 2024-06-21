using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SharedLibrary.Models.DtoModels.MainModels;

public class OrderItemDTO
{
    [Key]
    public Guid OrderItemDtoId { get; set; } = Guid.NewGuid();
    public uint Quantity { get; set; }

    public decimal SummaryItemPrice
    {
        get
        {
            decimal pricePerUnit = (decimal)(ProductDto?.PricePerUnit ??
                                              throw new InvalidOperationException());

            return pricePerUnit * Quantity;
        }
    }
    
    #region Constructors

    public OrderItemDTO(OrderDTO orderDto, ProductDTO productDto, uint quantity = 1)
    {
        OrderDtoId = orderDto.OrderDtoId;
        OrderDto = orderDto;
        
        OrderItemDtoId = Guid.NewGuid();

        ProductDtoId = productDto.ProductDtoId;
        ProductDto = productDto;

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