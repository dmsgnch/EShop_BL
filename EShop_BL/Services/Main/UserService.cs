using EShop_BL.Components;
using EShop_BL.Helpers;
using EShop_BL.Services.Main.Abstract;
using Newtonsoft.Json;
using SharedLibrary.Models.DbModels.MainModels;
using SharedLibrary.Requests;
using SharedLibrary.Responses;
using SharedLibrary.Routes;

namespace EShop_BL.Services.Main;

public class UserService : IUserService
{
    private readonly IConfiguration _configuration;
    private readonly IHttpClientService _httpClient;
    private readonly IHashProvider _hashProvider;

    public UserService(IConfiguration configuration, IHttpClientService httpClient, IHashProvider hashProvider)
    {
        _configuration = configuration;
        _httpClient = httpClient;
        _hashProvider = hashProvider;
    }

    public async Task<LambdaResponse<User>> EditUserAsync(EditUserRequest request)
    {
        var user = new User()
        {
            UserId = request.UserId,

            Name = request.Name,
            LastName = request.LastName,
            Patronymic = request.Patronymic,

            Email = request.Email,
            PhoneNumber = request.PhoneNumber,

            RoleId = request.RoleId,
            SellerId = request.SellerId,
        };

        if (!String.IsNullOrEmpty(request.Password))
        {
            user.PasswordHash = request.Password;
            user.ProvideSaltAndHash(_hashProvider);
        }
        else
        {
            user.PasswordHash = "";
            user.Salt = "";
        }

        var userResponse = await _httpClient.SendRequestAsync(new RestRequestForm(
            ApiRoutesDb.Controllers.UserContr + ApiRoutesDb.UniversalActions.UpdatePath,
            HttpMethod.Put, jsonData: JsonConvert.SerializeObject(user)));

        return await ResponseHandlerAsync(userResponse);
    }

    public async Task<LambdaResponse<User>> GetUserByIdAsync(string userId)
    {
        var userResponse = await _httpClient.SendRequestAsync(new RestRequestForm(
            ApiRoutesDb.Controllers.UserContr + ApiRoutesDb.UniversalActions.GetByIdPath + userId,
            HttpMethod.Get));

        return await ResponseHandlerAsync(userResponse);
    }

    private async Task<LambdaResponse<User>> ResponseHandlerAsync(HttpResponseMessage response)
    {
        var serverResponse = await JsonHelper.GetTypeFromResponseAsync<LambdaResponse<User>>(response);

        if (!response.IsSuccessStatusCode)
        {
            return new LambdaResponse<User>(errorInfo: serverResponse.ErrorInfo);
        }

        if (serverResponse.ResponseObject is null) throw new Exception("Response object is null");

        return new LambdaResponse<User>(responseObject: serverResponse.ResponseObject, info: serverResponse.Info);
    }
}