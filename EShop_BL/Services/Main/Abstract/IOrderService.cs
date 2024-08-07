using SharedLibrary.Models.ClientDtoModels.MainModels;
using SharedLibrary.Models.DtoModels.MainModels;
using SharedLibrary.Requests;
using SharedLibrary.Responses;

namespace EShop_BL.Services.Secondary.Abstract;

public interface IOrderService
{
    public Task<UniversalResponse> AddProductAsync(ProductCartRequest request);
    public Task<UniversalResponse> CreateOrderAsync(Guid userId);
    public Task<UniversalResponse> DeleteProductAsync(ProductCartRequest request);
    public Task<UniversalResponse<OrderCDTO>> GetCartAsync(Guid id);
    public Task<UniversalResponse<List<OrderCDTO>>> GetOrdersAsync(Guid id);
}