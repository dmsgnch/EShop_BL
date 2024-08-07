using SharedLibrary.Models.ClientDtoModels.MainModels;
using SharedLibrary.Responses;

namespace EShop_BL.Services.Secondary.Abstract;

public interface IUserService
{
    public Task<UniversalResponse<UserCDTO>> EditUserAsync(UserCDTO userCDto);
    public Task<UniversalResponse<UserCDTO>> GetUserByIdAsync(Guid userId);
}