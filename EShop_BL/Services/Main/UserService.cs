using System.IdentityModel.Tokens.Jwt;
using EShop_BL.Common.Constants;
using EShop_BL.Components;
using EShop_BL.Helpers;
using EShop_BL.Services.Main.Abstract;
using SharedLibrary.Models.MainModels;
using SharedLibrary.Requests;
using SharedLibrary.Responses;
using SharedLibrary.Routes;

namespace EShop_BL.Services.Main;

public class UserService : IUserService
{
    private readonly IConfiguration _configuration;
    private readonly IHttpClientService _httpClient;

    public UserService(IConfiguration configuration, IHttpClientService httpClient)
    {
        _configuration = configuration;
        _httpClient = httpClient;
    }
    
    public async Task<LambdaResponse<User>> GetUserByIdAsync(string userId)
    {
        var userResponse = await _httpClient.SendRequestAsync(new RestRequestForm(
                ApiRoutesDb.Controllers.User + ApiRoutesDb.Universal.GetById + userId,
                HttpMethod.Get));

        return await ResponseHandlerAsync(userResponse);
    }
    
    private async Task<LambdaResponse<User>> ResponseHandlerAsync(HttpResponseMessage response)
    {
        if (!response.IsSuccessStatusCode)
        {
            var errorResponse = await JsonHelper.GetTypeFromResponseAsync<LambdaResponse>(response);

            return new LambdaResponse<User>(info: errorResponse.Info);
        }

        var successResponse = await JsonHelper.GetTypeFromResponseAsync<LambdaResponse<User>>(response);

        User userResponse = successResponse.ResponseObject ?? throw new Exception("User is null");

        return new LambdaResponse<User>(responseObject: userResponse);
    }
}