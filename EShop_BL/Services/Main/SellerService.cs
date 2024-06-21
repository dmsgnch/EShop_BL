using EShop_BL.Components;
using EShop_BL.Helpers;
using EShop_BL.Services.Main.Abstract;
using Newtonsoft.Json;
using SharedLibrary.Models.DbModels.MainModels;
using SharedLibrary.Requests;
using SharedLibrary.Responses;
using SharedLibrary.Routes;

namespace EShop_BL.Services.Main;

public class SellerService : ISellerService
{
    private readonly HttpClientService _httpClient;

    public SellerService(HttpClientService httpClientService)
    {
        _httpClient = httpClientService;
    }
    
    public async Task<LambdaResponse<Seller>> CreateSellerAsync(EditSellerRequest request)
    {
        var seller = new Seller()
        {
            SellerId = new Guid(),

            CompanyName = request.CompanyName,
            ContactNumber = request.ContactNumber,
            EmailAddress = request.EmailAddress,

            CompanyDescription = request.CompanyDescription,
            ImageUrl = request.ImageUrl,
            AdditionNumber = request.AdditionNumber,
        };
        
        var userResponse = await _httpClient.SendRequestAsync(new RestRequestForm(
            ApiRoutesDb.Controllers.SellerContr + ApiRoutesDb.UniversalActions.CreatePath,
            HttpMethod.Post, jsonData: JsonConvert.SerializeObject(seller)));

        return await ResponseHandlerAsync<Seller>(userResponse);
    }
    
    public async Task<LambdaResponse> DeleteSellerAsync(Guid id)
    {
        var userResponse = await _httpClient.SendRequestAsync(new RestRequestForm(
            ApiRoutesDb.Controllers.SellerContr + ApiRoutesDb.UniversalActions.DeletePath + id,
            HttpMethod.Delete));

        return await EmptyResponseHandlerAsync(userResponse);
    }
    
    public async Task<LambdaResponse<Seller>> EditSellerAsync(EditSellerRequest request)
    {
        var seller = new Seller()
        {
            SellerId = request.SellerId,

            CompanyName = request.CompanyName,
            ContactNumber = request.ContactNumber,
            EmailAddress = request.EmailAddress,

            CompanyDescription = request.CompanyDescription,
            ImageUrl = request.ImageUrl,
            AdditionNumber = request.AdditionNumber,
        };
        
        var userResponse = await _httpClient.SendRequestAsync(new RestRequestForm(
            ApiRoutesDb.Controllers.SellerContr + ApiRoutesDb.UniversalActions.UpdatePath,
            HttpMethod.Put, jsonData: JsonConvert.SerializeObject(seller)));

        return await ResponseHandlerAsync<Seller>(userResponse);
    }
    
    public async Task<LambdaResponse<Seller>> GetSellerByIdAsync(Guid id)
    {
        var userResponse = await _httpClient.SendRequestAsync(new RestRequestForm(
            ApiRoutesDb.Controllers.SellerContr + ApiRoutesDb.UniversalActions.GetByIdPath + id,
            HttpMethod.Get));

        return await ResponseHandlerAsync<Seller>(userResponse);
    }

    public async Task<LambdaResponse<List<Seller>>> GetAllSellersAsync()
    {
        var userResponse = await _httpClient.SendRequestAsync(new RestRequestForm(
            ApiRoutesDb.Controllers.SellerContr + ApiRoutesDb.UniversalActions.GetAllPath,
            HttpMethod.Get));

        return await ResponseHandlerAsync<List<Seller>>(userResponse);
    }
    
    public async Task<LambdaResponse<string>> GetSellerByUserIdAsync(Guid id)
    {
        var response = await _httpClient.SendRequestAsync(new RestRequestForm(
            ApiRoutesDb.Controllers.UserContr + ApiRoutesDb.UniversalActions.GetAllPath,
            HttpMethod.Get));

        var serverResponse = await JsonHelper.GetTypeFromResponseAsync<LambdaResponse<List<User>>>(response);

        if (!response.IsSuccessStatusCode)
        {
            return new LambdaResponse<string>(errorInfo: serverResponse.ErrorInfo);
        }

        if (serverResponse.ResponseObject is null) throw new Exception("Response object is null");

        var user = serverResponse.ResponseObject.FirstOrDefault(u => u.UserId.Equals(id));
        
        if(user is null) throw new Exception("User was not found");
        
        var responseSeller = await _httpClient.SendRequestAsync(new RestRequestForm(
            ApiRoutesDb.Controllers.SellerContr + ApiRoutesDb.UniversalActions.GetByIdPath + user.SellerId,
            HttpMethod.Get));

        var sellerResponse = await JsonHelper.GetTypeFromResponseAsync<LambdaResponse<Seller>>(responseSeller);

        if (!responseSeller.IsSuccessStatusCode)
        {
            return new LambdaResponse<string>(errorInfo: sellerResponse.ErrorInfo);
        }

        if (sellerResponse.ResponseObject is null) throw new Exception("Response object is null");

        return new LambdaResponse<string>(responseObject: sellerResponse.ResponseObject.SellerId.ToString(), info: serverResponse.Info);
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