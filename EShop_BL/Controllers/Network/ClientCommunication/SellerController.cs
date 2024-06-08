using EShop_BL.Helpers;
using EShop_BL.Models.MainModels;
using EShop_BL.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace EShop_BL.Controllers.Network.ClientCommunication;

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
            var res = await JsonHelper.GetTypeFromResponse<List<Seller>>(response);
            
            return Ok(res);
        }
        
        return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
    }
}