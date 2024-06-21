using SharedLibrary.Requests;
using SharedLibrary.Responses;

namespace EShop_BL.Services.Main.Abstract;

public interface IAuthenticationService
{
    Task<LambdaResponse<string>> RegisterAsync(RegisterRequest registerRequest);
    Task<LambdaResponse<string>> LoginAsync(string email, string password);
}
