using System.IdentityModel.Tokens.Jwt;
using EShop_BL.Services.Main.Abstract;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SharedLibrary.Requests;
using SharedLibrary.Responses;
using SharedLibrary.Routes;

namespace EShop_BL.Controllers;

[ApiController]
[Route(ApiRoutes.Controllers.UserContr)]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPut(ApiRoutes.UserActions.EditPath)]
    public async Task<IActionResult> EditUserAsync([FromBody] EditUserRequest request)
    {
        var result = await _userService.EditUserAsync(request);
        return result.ResponseObject is null ? BadRequest(result) : Ok(result);
    }
    
    [HttpGet(ApiRoutes.UserActions.GetByIdPath)]
    public async Task<IActionResult> GetByIdAsync([FromBody] string userId)
    {
        var result = await _userService.GetUserByIdAsync(userId);
        return result.ResponseObject is null ? BadRequest(result) : Ok(result);
    }
}