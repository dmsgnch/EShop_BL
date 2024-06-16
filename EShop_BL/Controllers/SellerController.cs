using EShop_BL.Helpers;
using EShop_BL.Services.Main.Abstract;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Models.MainModels;

namespace EShop_BL.Controllers;

[ApiController, Route("seller")]
public class SellerController : ControllerBase
{
    private readonly ISellerService _sellerService;
    
    public SellerController(ISellerService sellerService)
    {
        _sellerService = sellerService;
    }
    
    [HttpGet, Route("getAll")]
    public async Task<IActionResult> GetSellerList()
    {
        var response = await _sellerService.GetDbResponseGetAllSellersAsync();

        if (response.IsSuccessStatusCode)
        {
            var res = await JsonHelper.GetTypeFromResponseAsync<List<Seller>>(response);
            
            return Ok(res);
        }
        
        return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
    }
}