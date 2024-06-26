namespace SharedLibrary.Models.ClientDtoModels.MainModels;

public class ProductCDTO
{
    public Guid ProductCDtoId { get; set; }

    public string? ImageUrl { get; set; }

    public string Name { get; set; }
    public string Description { get; set; }
    public decimal PricePerUnit { get; set; }
    public int WeightInGrams { get; set; }
    public int? InStock { get; set; }
    
    public ProductCDTO(
        string name, 
        string description, 
        decimal pricePerUnit, 
        int weightInGrams, 
        Guid sellerId,
        string? imageUrl = null)
    {
        Name = name;
        Description = description;
        PricePerUnit = pricePerUnit;
        WeightInGrams = weightInGrams;

        ImageUrl = imageUrl;
        
        SellerCDtoId = sellerId;
    }

    #region Relationships
    
    //Seller
    public Guid SellerCDtoId { get; set; }
    public SellerCDTO? SellerCDto { get; set; }
    
    //CartItem
    public List<OrderItemCDTO>? OrderItemsCDto { get; set; } = new();

    #endregion
}