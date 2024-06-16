using System.IdentityModel.Tokens.Jwt;
using SharedLibrary.Responses;

namespace EShop_BL.Services.Main.Abstract;

public interface IUserService
{
    public Task<GetUserResponse> GetUserByTokenAsync(string stringToken);
}