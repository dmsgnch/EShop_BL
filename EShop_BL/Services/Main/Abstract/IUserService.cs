using System.IdentityModel.Tokens.Jwt;
using SharedLibrary.Models.MainModels;
using SharedLibrary.Responses;

namespace EShop_BL.Services.Main.Abstract;

public interface IUserService
{
    public Task<LambdaResponse<User>> GetUserByIdAsync(string userId);
}