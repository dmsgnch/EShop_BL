using System.IdentityModel.Tokens.Jwt;
using EShop_BL.Services.Main.Abstract;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SharedLibrary.Requests;
using SharedLibrary.Responses;
using SharedLibrary.Routes;

namespace EShop_BL.Controllers;

[ApiController]
[Route(ApiRoutes.Controllers.User)]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet(ApiRoutes.User.GetById)]
    public async Task<IActionResult> GetByIdAsync([FromBody] string userId)
    {
        var result = await _userService.GetUserByIdAsync(userId);
        return result.ResponseObject is null ? BadRequest(result) : Ok(result);
    }
}