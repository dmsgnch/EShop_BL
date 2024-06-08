using SharedLibrary.Requests;
using SharedLibrary.Responses;

namespace EShop_BL.Services.Abstract;

public interface IAuthenticationService
{
    Task<AuthenticationResponse> RegisterAsync(RegisterRequest registerRequest);
    Task<AuthenticationResponse> LoginAsync(string email, string password);
}
