using EShop_BL.Services.Main.Abstract;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Requests;
using SharedLibrary.Routes;

namespace EShop_BL.Controllers;

[ApiController]
[Route(ApiRoutes.Controllers.SellerContr)]
public class SellerController : ControllerBase
{
    private readonly ISellerService _sellerService;
    
    public SellerController(ISellerService sellerService)
    {
        _sellerService = sellerService;
    }
    
    [HttpPost(ApiRoutes.SellerActions.CreatePath)]
    public async Task<IActionResult> CreateSeller([FromBody]EditSellerRequest request)
    {
        var result = await _sellerService.CreateSellerAsync(request);
        return result.ResponseObject is null ? BadRequest(result) : Ok(result);
    }
    
    [HttpDelete(ApiRoutes.SellerActions.DeletePath)]
    public async Task<IActionResult> DeleteSeller([FromBody]Guid id)
    {
        var result = await _sellerService.DeleteSellerAsync(id);
        return result.Info is null ? BadRequest(result) : Ok(result);
    }
    
    [HttpPut(ApiRoutes.SellerActions.EditPath)]
    public async Task<IActionResult> EditSeller([FromBody]EditSellerRequest request)
    {
        var result = await _sellerService.EditSellerAsync(request);
        return result.ResponseObject is null ? BadRequest(result) : Ok(result);
    }
    
    [HttpGet(ApiRoutes.SellerActions.GetByIdPath)]
    public async Task<IActionResult> GetSellerById([FromBody]Guid id)
    {
        var result = await _sellerService.GetSellerByIdAsync(id);
        return result.ResponseObject is null ? BadRequest(result) : Ok(result);
    }
    
    [HttpGet(ApiRoutes.SellerActions.GetAllPath)]
    public async Task<IActionResult> GetAllSellers()
    {
        var result = await _sellerService.GetAllSellersAsync();
        return result.ResponseObject is null ? BadRequest(result) : Ok(result);
    }
    
    [HttpGet(ApiRoutes.SellerActions.GetSellerIdByUserIdPath)]
    public async Task<IActionResult> GetSellerByUserId([FromBody]Guid id)
    {
        var result = await _sellerService.GetSellerByUserIdAsync(id);
        return result.ResponseObject is null ? BadRequest(result) : Ok(result);
    }
}