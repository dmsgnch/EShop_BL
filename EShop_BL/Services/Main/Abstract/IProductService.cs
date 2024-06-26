using SharedLibrary.Models.ClientDtoModels.MainModels;
using SharedLibrary.Responses;

namespace EShop_BL.Services.Main.Abstract;

public interface IProductService
{
    public Task<UniversalResponse<ProductCDTO>> CreateProductAsync(ProductCDTO productCDto);
    public Task<UniversalResponse> DeleteProductAsync(Guid id);
    public Task<UniversalResponse<ProductCDTO>> EditProductAsync(ProductCDTO productCDto);
    public Task<UniversalResponse<ProductCDTO>> GetProductByIdAsync(Guid id);
    public Task<UniversalResponse<List<ProductCDTO>>> GetAllProductsAsync();
    public Task<UniversalResponse<List<ProductCDTO>>> GetAllProductsBySellerIdAsync(Guid id);
}