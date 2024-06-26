using EShop_BL.Components;
using EShop_BL.Extensions;
using EShop_BL.Helpers;
using EShop_BL.Services.Main.Abstract;
using Newtonsoft.Json;
using SharedLibrary.Models.ClientDtoModels.MainModels;
using SharedLibrary.Models.DtoModels.MainModels;
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

    public async Task<UniversalResponse<UserCDTO>> EditUserAsync(UserCDTO userCDto)
    {
        var user = userCDto.ToUserDto();
        if (!String.IsNullOrEmpty(user.PasswordHash))
        {
            user.ProvideSaltAndHash(_hashProvider);
        }

        var userResponse = await _httpClient.SendRequestAsync(new RestRequestForm(
            endPoint: ApiRoutesDb.Controllers.UserContr + ApiRoutesDb.UniversalActions.UpdateAction,
            requestMethod: HttpMethod.Put, 
            jsonData: JsonConvert.SerializeObject(user)));

        return await ResponseHandlerAsync(userResponse);
    }

    public async Task<UniversalResponse<UserCDTO>> GetUserByIdAsync(Guid userId)
    {
        var userResponse = await _httpClient.SendRequestAsync(new RestRequestForm(
            endPoint: ApiRoutesDb.Controllers.UserContr + ApiRoutesDb.UniversalActions.GetByIdAction,
            requestMethod: HttpMethod.Get,
            jsonData: JsonConvert.SerializeObject(userId)));

        return await ResponseHandlerAsync(userResponse);
    }

    private async Task<UniversalResponse<UserCDTO>> ResponseHandlerAsync(HttpResponseMessage response)
    {
        var serverResponse = await JsonHelper.GetTypeFromResponseAsync<UniversalResponse<UserDTO>>(response);

        if (!response.IsSuccessStatusCode)
        {
            return new UniversalResponse<UserCDTO>(errorInfo: serverResponse.ErrorInfo);
        }

        if (serverResponse.ResponseObject is null) throw new Exception("Response object is null");

        return new UniversalResponse<UserCDTO>(responseObject: serverResponse.ResponseObject.ToUserCDto(), info: serverResponse.Info);
    }
}