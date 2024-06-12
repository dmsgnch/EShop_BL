using EShop_BL.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SharedLibrary.Requests;
using SharedLibrary.Responses;
using SharedLibrary.Routes;

namespace EShop_BL.Controllers.Network.ClientCommunication;

[ApiController]
[Route(ApiRoutes.Controllers.Authentication)]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;

    public AuthenticationController(IAuthenticationService authService)
    {
        _authenticationService = authService;
    }

    [HttpPost(ApiRoutes.Authentication.Register)]
    public async Task<IActionResult> RegisterAsync([FromBody] RegisterRequest regRequest)
    {
        var result = await _authenticationService.RegisterAsync(regRequest);
        return result.Token is null ? BadRequest(result) : Ok(result);
    }
    [HttpPost(ApiRoutes.Authentication.Login)]
    public async Task<IActionResult> LoginAsync([FromBody] LoginRequest request)
    {
        var result = await _authenticationService.LoginAsync(request.Email, request.Password);
        return result.Token is null ? BadRequest(result) : Ok(result);
    }
}