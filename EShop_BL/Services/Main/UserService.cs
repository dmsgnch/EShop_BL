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
    
    public async Task<GetUserResponse> GetUserByTokenAsync(string stringToken)
    {
        var handler = new JwtSecurityTokenHandler();
        
        var token = handler.ReadToken(stringToken) as JwtSecurityToken;

        if (token is null)
        {
            return new GetUserResponse(info: ErrorMessages.User.TokenIsIncorrect);;
        }

        var userId = token.Claims.First(claim => claim.Type == "id").Value ??
                     throw new Exception("User id not found in token");

        var userResponse = await _httpClient.SendRequestAsync(new RestRequestForm(
                ApiRoutesDb.Controllers.User + ApiRoutesDb.Universal.GetById + userId,
                HttpMethod.Get));

        return await ResponseHandlerAsync(userResponse);
    }
    
    private async Task<GetUserResponse> ResponseHandlerAsync(HttpResponseMessage response)
    {
        if (!response.IsSuccessStatusCode)
        {
            var errorResponse = await JsonHelper.GetTypeFromResponseAsync<LambdaResponse>(response);

            return new GetUserResponse(info: errorResponse.Info);
        }

        var userResponse = await JsonHelper.GetTypeFromResponseAsync<User>(response);

        return new GetUserResponse(user: userResponse);
    }
}