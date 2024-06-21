namespace SharedLibrary.Requests;

public class ProductCartRequest
{
    public Guid? CartId { get; set; }
    public Guid ProductId { get; set; }
    public Guid UserId { get; set; }

    public ProductCartRequest()
    {
        
    }
    
    public ProductCartRequest(Guid productId, Guid userId, Guid? cartId = null)
    {
        ProductId = productId;
        UserId = userId;
        CartId = cartId;
    }
}