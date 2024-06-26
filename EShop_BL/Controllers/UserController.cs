using System.IdentityModel.Tokens.Jwt;
using EShop_BL.Services.Main.Abstract;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SharedLibrary.Models.ClientDtoModels.MainModels;
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

    [HttpPut(ApiRoutes.UniversalActions.EditAction)]
    public async Task<IActionResult> EditUserAsync([FromBody] UserCDTO userCDto)
    {
        var result = await _userService.EditUserAsync(userCDto);
        return result.ResponseObject is null ? BadRequest(result) : Ok(result);
    }
    
    [HttpGet(ApiRoutes.UniversalActions.GetByIdAction)]
    public async Task<IActionResult> GetByIdAsync([FromBody] Guid userId)
    {
        var result = await _userService.GetUserByIdAsync(userId);
        return result.ResponseObject is null ? BadRequest(result) : Ok(result);
    }
}