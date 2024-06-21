using SharedLibrary.Models.DbModels.MainModels;
using SharedLibrary.Requests;
using SharedLibrary.Responses;

namespace EShop_BL.Services.Main.Abstract;

public interface ISellerService
{
    public Task<LambdaResponse<Seller>> CreateSellerAsync(EditSellerRequest request);
    public Task<LambdaResponse> DeleteSellerAsync(Guid id);
    public Task<LambdaResponse<Seller>> EditSellerAsync(EditSellerRequest request);
    public Task<LambdaResponse<Seller>> GetSellerByIdAsync(Guid id);
    public Task<LambdaResponse<List<Seller>>> GetAllSellersAsync();
    public Task<LambdaResponse<string>> GetSellerByUserIdAsync(Guid id);
}