using SharedLibrary.Models.DbModels.MainModels;
using SharedLibrary.Requests;
using SharedLibrary.Responses;

namespace EShop_BL.Services.Main.Abstract;

public interface IUserService
{
    public Task<LambdaResponse<User>> EditUserAsync(EditUserRequest request);
    public Task<LambdaResponse<User>> GetUserByIdAsync(string userId);
}