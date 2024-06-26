using EShop_BL.Services.Main.Abstract;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SharedLibrary.Requests;
using SharedLibrary.Responses;
using SharedLibrary.Routes;

namespace EShop_BL.Controllers;

[ApiController]
[Route(ApiRoutes.Controllers.AuthenticationContr)]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;

    public AuthenticationController(IAuthenticationService authService)
    {
        _authenticationService = authService;
    }

    [HttpPost(ApiRoutes.AuthenticationActions.RegisterAction)]
    public async Task<IActionResult> RegisterAsync([FromBody] RegisterRequest regRequest)
    {
        var result = await _authenticationService.RegisterAsync(regRequest);
        return result.ResponseObject is null ? BadRequest(result) : Ok(result);
    }

    [HttpPost(ApiRoutes.AuthenticationActions.LoginAction)]
    public async Task<IActionResult> LoginAsync([FromBody] LoginRequest request)
    {
        var result = await _authenticationService.LoginAsync(request.Email, request.Password);
        result.ResponseObject ??= result.ResponseObject = "";
        return String.IsNullOrEmpty(result.ResponseObject)  ? BadRequest(result) : Ok(result);
    }
}