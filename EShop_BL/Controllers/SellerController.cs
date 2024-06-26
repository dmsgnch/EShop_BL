using EShop_BL.Services.Main.Abstract;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Models.ClientDtoModels.MainModels;
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
    
    [HttpPost(ApiRoutes.UniversalActions.CreateAction)]
    public async Task<IActionResult> CreateSeller([FromBody]SellerCDTO sellerCdto)
    {
        var result = await _sellerService.CreateSellerAsync(sellerCdto);
        return result.ResponseObject is null ? BadRequest(result) : Ok(result);
    }
    
    [HttpDelete(ApiRoutes.UniversalActions.DeleteAction)]
    public async Task<IActionResult> DeleteSeller([FromBody]Guid id)
    {
        var result = await _sellerService.DeleteSellerAsync(id);
        return result.Info is null ? BadRequest(result) : Ok(result);
    }
    
    [HttpPut(ApiRoutes.UniversalActions.EditAction)]
    public async Task<IActionResult> EditSeller([FromBody]SellerCDTO sellerCdto)
    {
        var result = await _sellerService.EditSellerAsync(sellerCdto);
        return result.ResponseObject is null ? BadRequest(result) : Ok(result);
    }
    
    [HttpGet(ApiRoutes.UniversalActions.GetByIdAction)]
    public async Task<IActionResult> GetSellerById([FromBody]Guid id)
    {
        var result = await _sellerService.GetSellerByIdAsync(id);
        return result.ResponseObject is null ? BadRequest(result) : Ok(result);
    }
    
    [HttpGet(ApiRoutes.UniversalActions.GetAllAction)]
    public async Task<IActionResult> GetAllSellers()
    {
        var result = await _sellerService.GetAllSellersAsync();
        return result.ResponseObject is null ? BadRequest(result) : Ok(result);
    }
    
    [HttpGet(ApiRoutes.SellerActions.GetSellerIdByUserIdAction)]
    public async Task<IActionResult> GetSellerByUserId([FromBody]Guid id)
    {
        var result = await _sellerService.GetSellerIdByUserIdAsync(id);
        return result.ResponseObject is null ? BadRequest(result) : Ok(result);
    }
}