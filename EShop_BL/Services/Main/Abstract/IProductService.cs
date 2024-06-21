using SharedLibrary.Models.DbModels.MainModels;
using SharedLibrary.Requests;
using SharedLibrary.Responses;

namespace EShop_BL.Services.Main.Abstract;

public interface IProductService
{
    public Task<LambdaResponse<Product>> CreateProductAsync(EditProductRequest request);
    public Task<LambdaResponse> DeleteProductAsync(Guid id);
    public Task<LambdaResponse<Product>> EditProductAsync(EditProductRequest request);
    public Task<LambdaResponse<Product>> GetProductByIdAsync(Guid id);
    public Task<LambdaResponse<List<Product>>> GetAllProductsAsync();
    public Task<LambdaResponse<List<Product>>> GetAllProductsBySellerIdAsync(Guid id);
}