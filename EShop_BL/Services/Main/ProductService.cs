using EShop_BL.Components;
using EShop_BL.Helpers;
using EShop_BL.Services.Main.Abstract;
using Newtonsoft.Json;
using SharedLibrary.Models.DbModels.MainModels;
using SharedLibrary.Requests;
using SharedLibrary.Responses;
using SharedLibrary.Routes;

namespace EShop_BL.Services.Main;

public class ProductService : IProductService
{
    private readonly HttpClientService _httpClient;

    public ProductService(HttpClientService httpClientService)
    {
        _httpClient = httpClientService;
    }
    
    public async Task<LambdaResponse<Product>> CreateProductAsync(EditProductRequest request)
    {
        var product = new Product()
        {
            ProductId = new Guid(),

            Name = request.Name,
            WeightInGrams = request.WeightInGrams,
            PricePerUnit = request.PricePerUnit,

            Description = request.Description,
            ImageUrl = request.ImageUrl,
            InStock = request.InStock ?? 1,
            
            SellerId = request.SellerId,
        };
        
        var productResponse = await _httpClient.SendRequestAsync(new RestRequestForm(
            ApiRoutesDb.Controllers.ProductContr + ApiRoutesDb.UniversalActions.CreatePath,
            HttpMethod.Post, jsonData: JsonConvert.SerializeObject(product)));

        return await ResponseHandlerAsync<Product>(productResponse);
    }
    
    public async Task<LambdaResponse> DeleteProductAsync(Guid id)
    {
        var productResponse = await _httpClient.SendRequestAsync(new RestRequestForm(
            ApiRoutesDb.Controllers.ProductContr + ApiRoutesDb.UniversalActions.DeletePath + id,
            HttpMethod.Delete));

        return await EmptyResponseHandlerAsync(productResponse);
    }
    
    public async Task<LambdaResponse<Product>> EditProductAsync(EditProductRequest request)
    {
        var product = new Product()
        {
            ProductId = new Guid(),

            Name = request.Name,
            WeightInGrams = request.WeightInGrams,
            PricePerUnit = request.PricePerUnit,

            Description = request.Description,
            ImageUrl = request.ImageUrl,
            InStock = request.InStock ?? -1,
            
            SellerId = request.SellerId,
        };
        
        var productResponse = await _httpClient.SendRequestAsync(new RestRequestForm(
            ApiRoutesDb.Controllers.ProductContr + ApiRoutesDb.UniversalActions.UpdatePath,
            HttpMethod.Put, jsonData: JsonConvert.SerializeObject(product)));

        return await ResponseHandlerAsync<Product>(productResponse);
    }
    
    public async Task<LambdaResponse<Product>> GetProductByIdAsync(Guid id)
    {
        var productResponse = await _httpClient.SendRequestAsync(new RestRequestForm(
            ApiRoutesDb.Controllers.ProductContr + ApiRoutesDb.UniversalActions.GetByIdPath + id,
            HttpMethod.Get));

        return await ResponseHandlerAsync<Product>(productResponse);
    }

    public async Task<LambdaResponse<List<Product>>> GetAllProductsAsync()
    {
        var productResponse = await _httpClient.SendRequestAsync(new RestRequestForm(
            ApiRoutesDb.Controllers.ProductContr + ApiRoutesDb.UniversalActions.GetAllPath,
            HttpMethod.Get));

        return await ResponseHandlerAsync<List<Product>>(productResponse);
    }

    public async Task<LambdaResponse<List<Product>>> GetAllProductsBySellerIdAsync(Guid id)
    {
        var productResponse = await _httpClient.SendRequestAsync(new RestRequestForm(
            ApiRoutesDb.Controllers.ProductContr + ApiRoutesDb.UniversalActions.GetAllPath,
            HttpMethod.Get));

        var serverResponse = await JsonHelper.GetTypeFromResponseAsync<LambdaResponse<List<Product>>>(productResponse);

        if (!productResponse.IsSuccessStatusCode)
        {
            return new LambdaResponse<List<Product>>(errorInfo: serverResponse.ErrorInfo);
        }

        if (serverResponse.ResponseObject is null) throw new Exception("Response object is null");
        
        var userResponse = await _httpClient.SendRequestAsync(new RestRequestForm(
            ApiRoutesDb.Controllers.UserContr + ApiRoutesDb.UniversalActions.GetByIdPath + id,
            HttpMethod.Get));

        var serverUserResponse = await JsonHelper.GetTypeFromResponseAsync<LambdaResponse<User>>(userResponse);

        if (!userResponse.IsSuccessStatusCode)
        {
            return new LambdaResponse<List<Product>>(errorInfo: serverUserResponse.ErrorInfo);
        }

        if (serverUserResponse.ResponseObject is null) throw new Exception("Response object is null");

        var sellerProductList = serverResponse.ResponseObject.Where(prdts => prdts.SellerId.Equals(serverUserResponse.ResponseObject.SellerId)).ToList();

        return new LambdaResponse<List<Product>>(responseObject: sellerProductList, info: serverResponse.Info);
    }
    
    private async Task<LambdaResponse<T>> ResponseHandlerAsync<T>(HttpResponseMessage response) where T : class
    {
        var serverResponse = await JsonHelper.GetTypeFromResponseAsync<LambdaResponse<T>>(response);

        if (!response.IsSuccessStatusCode)
        {
            return new LambdaResponse<T>(errorInfo: serverResponse.ErrorInfo);
        }

        if (serverResponse.ResponseObject is null) throw new Exception("Response object is null");

        return new LambdaResponse<T>(responseObject: serverResponse.ResponseObject as T, info: serverResponse.Info);
    }
    
    private async Task<LambdaResponse> EmptyResponseHandlerAsync(HttpResponseMessage response)
    {
        var serverResponse = await JsonHelper.GetTypeFromResponseAsync<LambdaResponse>(response);

        if (!response.IsSuccessStatusCode)
        {
            return new LambdaResponse(errorInfo: serverResponse.ErrorInfo);
        }

        return new LambdaResponse(info: serverResponse.Info);
    }
}