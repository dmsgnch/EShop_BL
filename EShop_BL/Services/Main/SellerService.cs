using EShop_BL.Components;
using EShop_BL.Extensions;
using EShop_BL.Helpers;
using EShop_BL.Services.Secondary.Abstract;
using Newtonsoft.Json;
using SharedLibrary.Models.ClientDtoModels.MainModels;
using SharedLibrary.Models.DtoModels.MainModels;
using SharedLibrary.Responses;
using SharedLibrary.Routes;

namespace EShop_BL.Services.Secondary;

public class SellerService : ISellerService
{
    private readonly HttpClientServiceBase _httpClient;

    public SellerService(HttpClientServiceBase httpClientService)
    {
        _httpClient = httpClientService;
    }
    
    public async Task<UniversalResponse<SellerCDTO>> CreateSellerAsync(SellerCDTO sellerCdto)
    {
        var seller = sellerCdto.ToSellerDto();
        
        var userResponse = await _httpClient.ProcessRequestAsync(new HttpRequestForm(
            endPoint: ApiRoutesDb.Controllers.SellerContr + ApiRoutesDb.UniversalActions.CreateAction,
            requestMethod: HttpMethod.Post, 
            jsonData: JsonConvert.SerializeObject(seller)));

        var dtoResult = await ResponseHandlerAsync<SellerDTO>(userResponse);
        return new UniversalResponse<SellerCDTO>(
            info: dtoResult.Info, errorInfo: dtoResult.ErrorInfo, 
            responseObject: dtoResult.ResponseObject?.ToSellerCDto());
    }
    
    public async Task<UniversalResponse> DeleteSellerAsync(Guid id)
    {
        var userResponse = await _httpClient.ProcessRequestAsync(new HttpRequestForm(
            endPoint: ApiRoutesDb.Controllers.SellerContr + ApiRoutesDb.UniversalActions.DeleteAction,
            requestMethod: HttpMethod.Delete,
            jsonData: JsonConvert.SerializeObject(id)));

        return await EmptyResponseHandlerAsync(userResponse);
    }
    
    public async Task<UniversalResponse<SellerCDTO>> EditSellerAsync(SellerCDTO sellerCdto)
    {
        var seller = sellerCdto.ToSellerDto();
        
        var userResponse = await _httpClient.ProcessRequestAsync(new HttpRequestForm(
            endPoint: ApiRoutesDb.Controllers.SellerContr + ApiRoutesDb.UniversalActions.UpdateAction,
            requestMethod: HttpMethod.Put, 
            jsonData: JsonConvert.SerializeObject(seller)));

        var dtoResult = await ResponseHandlerAsync<SellerDTO>(userResponse);
        return new UniversalResponse<SellerCDTO>(
            info: dtoResult.Info, errorInfo: dtoResult.ErrorInfo, 
            responseObject: dtoResult.ResponseObject?.ToSellerCDto());
    }
    
    public async Task<UniversalResponse<SellerCDTO>> GetSellerByIdAsync(Guid id)
    {
        var userResponse = await _httpClient.ProcessRequestAsync(new HttpRequestForm(
            endPoint: ApiRoutesDb.Controllers.SellerContr + ApiRoutesDb.UniversalActions.GetByIdAction,
            requestMethod:HttpMethod.Get,
            jsonData: JsonConvert.SerializeObject(id)));

        var dtoResult = await ResponseHandlerAsync<SellerDTO>(userResponse);
        return new UniversalResponse<SellerCDTO>(
            info: dtoResult.Info, errorInfo: dtoResult.ErrorInfo, 
            responseObject: dtoResult.ResponseObject?.ToSellerCDto());
    }

    public async Task<UniversalResponse<List<SellerCDTO>>> GetAllSellersAsync()
    {
        var userResponse = await _httpClient.ProcessRequestAsync(new HttpRequestForm(
            endPoint: ApiRoutesDb.Controllers.SellerContr + ApiRoutesDb.UniversalActions.GetAllAction,
            requestMethod: HttpMethod.Get));

        var dtoResult = await ResponseHandlerAsync<List<SellerDTO>>(userResponse);
                return new UniversalResponse<List<SellerCDTO>>(
                    info: dtoResult.Info, errorInfo: dtoResult.ErrorInfo, 
                    responseObject: dtoResult.ResponseObject?.Select(p => p.ToSellerCDto()).ToList());
    }
    
    public async Task<UniversalResponse<string>> GetSellerIdByUserIdAsync(Guid id)
    {
        var getUsersRequest = await _httpClient.ProcessRequestAsync(new HttpRequestForm(
            endPoint: ApiRoutesDb.Controllers.UserContr + ApiRoutesDb.UniversalActions.GetAllAction,
            requestMethod: HttpMethod.Get));

        var getUsersResult = await JsonHelper.GetTypeFromResponseAsync<UniversalResponse<List<UserDTO>>>(getUsersRequest);

        if (!getUsersRequest.IsSuccessStatusCode)
        {
            return new UniversalResponse<string>(errorInfo: getUsersResult.ErrorInfo);
        }

        if (getUsersResult.ResponseObject is null) throw new Exception("Response object is null");
        
        var sellerId = getUsersResult.ResponseObject.FirstOrDefault(u => u.UserDtoId.Equals(id))?.SellerDtoId;
        if (sellerId is null) throw new Exception("Seller was not found");
        
        var response = await _httpClient.ProcessRequestAsync(new HttpRequestForm(
            endPoint: ApiRoutesDb.Controllers.SellerContr + ApiRoutesDb.UniversalActions.GetByIdAction,
            requestMethod: HttpMethod.Get,
            jsonData: JsonConvert.SerializeObject(sellerId)));

        var serverResponse = await JsonHelper.GetTypeFromResponseAsync<UniversalResponse<string>>(response);

        if (!response.IsSuccessStatusCode)
        {
            return new UniversalResponse<string>(errorInfo: serverResponse.ErrorInfo);
        }

        if (serverResponse.ResponseObject is null) throw new Exception("Response object is null");

        return new UniversalResponse<string>(responseObject: serverResponse.ResponseObject, info: serverResponse.Info);
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