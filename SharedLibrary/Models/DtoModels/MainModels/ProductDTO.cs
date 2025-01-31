namespace SharedLibrary.Models.DtoModels.MainModels;

public class ProductDTO
{
    public Guid ProductDtoId { get; set; }

    public string? ImageUrl { get; set; }

    public string Name { get; set; }
    public string Description { get; set; }
    public decimal PricePerUnit { get; set; }
    public int WeightInGrams { get; set; }
    public int? InStock { get; set; }
    
    #region Constructors

    public ProductDTO(
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

        SellerDtoId = sellerId; }
    
    #endregion

    #region Relationships
    
    //Seller
    public Guid SellerDtoId { get; set; }
    public SellerDTO? SellerDto { get; set; }
    
    //CartItem
    public List<OrderItemDTO>? OrderItemsDto { get; set; } = new();

    #endregion
}