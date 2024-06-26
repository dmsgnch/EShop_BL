using SharedLibrary.Models.ClientDtoModels.MainModels;
using SharedLibrary.Responses;

namespace EShop_BL.Services.Main.Abstract;

public interface ISellerService
{
    public Task<UniversalResponse<SellerCDTO>> CreateSellerAsync(SellerCDTO sellerCdto);
    public Task<UniversalResponse> DeleteSellerAsync(Guid id);
    public Task<UniversalResponse<SellerCDTO>> EditSellerAsync(SellerCDTO sellerCdto);
    public Task<UniversalResponse<SellerCDTO>> GetSellerByIdAsync(Guid id);
    public Task<UniversalResponse<List<SellerCDTO>>> GetAllSellersAsync();
    public Task<UniversalResponse<string>> GetSellerIdByUserIdAsync(Guid id);
}