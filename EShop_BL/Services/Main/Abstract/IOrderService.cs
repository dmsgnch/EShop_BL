using SharedLibrary.Models.DbModels.MainModels;
using SharedLibrary.Requests;
using SharedLibrary.Responses;

namespace EShop_BL.Services.Main.Abstract;

public interface IOrderService
{
    public Task<LambdaResponse> AddProductAsync(ProductCartRequest request);
    public Task<LambdaResponse> DeleteProductAsync(ProductCartRequest request);
    public Task<LambdaResponse<Order>> GetCartAsync(Guid id);
    public Task<LambdaResponse<List<Order>>> GetOrdersAsync(Guid id);
}