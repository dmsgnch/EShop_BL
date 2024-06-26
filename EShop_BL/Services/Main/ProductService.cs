using EShop_BL.Components;
using EShop_BL.Extensions;
using EShop_BL.Helpers;
using EShop_BL.Services.Main.Abstract;
using Newtonsoft.Json;
using SharedLibrary.Models.ClientDtoModels.MainModels;
using SharedLibrary.Models.DtoModels.MainModels;
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
    
    public async Task<UniversalResponse<ProductCDTO>> CreateProductAsync(ProductCDTO productCDto)
    {
        var product = productCDto.ToProductDto();
        
        var productResponse = await _httpClient.SendRequestAsync(new RestRequestForm(
            endPoint: ApiRoutesDb.Controllers.ProductContr + ApiRoutesDb.UniversalActions.CreateAction,
            requestMethod:HttpMethod.Post, 
            jsonData: JsonConvert.SerializeObject(product)));

        var dtoResult = await ResponseHandlerAsync<ProductDTO>(productResponse);
        return new UniversalResponse<ProductCDTO>(
            info: dtoResult.Info, errorInfo: dtoResult.ErrorInfo, 
            responseObject: dtoResult.ResponseObject?.ToProductCDto());
    }
    
    public async Task<UniversalResponse> DeleteProductAsync(Guid id)
    {
        var productResponse = await _httpClient.SendRequestAsync(new RestRequestForm(
            endPoint: ApiRoutesDb.Controllers.ProductContr + ApiRoutesDb.UniversalActions.DeleteAction,
            requestMethod: HttpMethod.Delete,
            jsonData: JsonConvert.SerializeObject(id)));

        return await EmptyResponseHandlerAsync(productResponse);
    }
    
    public async Task<UniversalResponse<ProductCDTO>> EditProductAsync(ProductCDTO productCdto)
    {
        var product = productCdto.ToProductDto();
        
        var productResponse = await _httpClient.SendRequestAsync(new RestRequestForm(
            endPoint: ApiRoutesDb.Controllers.ProductContr + ApiRoutesDb.UniversalActions.UpdateAction,
            requestMethod: HttpMethod.Put, 
            jsonData: JsonConvert.SerializeObject(product)));

        var dtoResult = await ResponseHandlerAsync<ProductDTO>(productResponse);
        return new UniversalResponse<ProductCDTO>(
            info: dtoResult.Info, errorInfo: dtoResult.ErrorInfo, 
            responseObject: dtoResult.ResponseObject?.ToProductCDto());
    }
    
    public async Task<UniversalResponse<ProductCDTO>> GetProductByIdAsync(Guid id)
    {
        var productResponse = await _httpClient.SendRequestAsync(new RestRequestForm(
            endPoint: ApiRoutesDb.Controllers.ProductContr + ApiRoutesDb.UniversalActions.GetByIdAction,
            requestMethod: HttpMethod.Get,
            jsonData: JsonConvert.SerializeObject(id)));

        var dtoResult = await ResponseHandlerAsync<ProductDTO>(productResponse);
        return new UniversalResponse<ProductCDTO>(
            info: dtoResult.Info, errorInfo: dtoResult.ErrorInfo, 
            responseObject: dtoResult.ResponseObject?.ToProductCDto());
    }

    public async Task<UniversalResponse<List<ProductCDTO>>> GetAllProductsAsync()
    {
        var productResponse = await _httpClient.SendRequestAsync(new RestRequestForm(
            endPoint: ApiRoutesDb.Controllers.ProductContr + ApiRoutesDb.UniversalActions.GetAllAction,
            requestMethod: HttpMethod.Get));

        var dtoResult = await ResponseHandlerAsync<List<ProductDTO>>(productResponse);
        return new UniversalResponse<List<ProductCDTO>>(
            info: dtoResult.Info, errorInfo: dtoResult.ErrorInfo, 
            responseObject: dtoResult.ResponseObject?.Select(p => p.ToProductCDto()).ToList());
    }

    public async Task<UniversalResponse<List<ProductCDTO>>> GetAllProductsBySellerIdAsync(Guid id)
    {
        var productResponse = await _httpClient.SendRequestAsync(new RestRequestForm(
            endPoint: ApiRoutesDb.Controllers.ProductContr + ApiRoutesDb.UniversalActions.GetAllAction,
            requestMethod: HttpMethod.Get));

        var serverResponse = await JsonHelper.GetTypeFromResponseAsync<UniversalResponse<List<ProductDTO>>>(productResponse);

        if (!productResponse.IsSuccessStatusCode)
        {
            return new UniversalResponse<List<ProductCDTO>>(errorInfo: serverResponse.ErrorInfo);
        }

        if (serverResponse.ResponseObject is null) throw new Exception("Response object is null");
        
        var sellerProductList = serverResponse.ResponseObject.Where(pr => pr.SellerDtoId.Equals(id)).ToList();

        return new UniversalResponse<List<ProductCDTO>>(responseObject: sellerProductList.Select(pl => pl.ToProductCDto()).ToList(), info: serverResponse.Info);
    }
    
    private async Task<UniversalResponse<T>> ResponseHandlerAsync<T>(HttpResponseMessage response) where T : class
    {
        var serverResponse = await JsonHelper.GetTypeFromResponseAsync<UniversalResponse<T>>(response);

        if (!response.IsSuccessStatusCode)
        {
            return new UniversalResponse<T>(errorInfo: serverResponse.ErrorInfo);
        }

        if (serverResponse.ResponseObject is null) throw new Exception("Response object is null");

        return new UniversalResponse<T>(responseObject: serverResponse.ResponseObject as T, info: serverResponse.Info);
    }
    
    private async Task<UniversalResponse> EmptyResponseHandlerAsync(HttpResponseMessage response)
    {
        var serverResponse = await JsonHelper.GetTypeFromResponseAsync<UniversalResponse>(response);

        if (!response.IsSuccessStatusCode)
        {
            return new UniversalResponse(errorInfo: serverResponse.ErrorInfo);
        }

        return new UniversalResponse(info: serverResponse.Info);
    }
}