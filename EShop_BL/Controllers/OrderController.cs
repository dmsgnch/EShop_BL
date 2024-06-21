using EShop_BL.Services.Main.Abstract;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Requests;
using SharedLibrary.Routes;

namespace EShop_BL.Controllers;

[ApiController]
[Route(ApiRoutes.Controllers.OrderContr)]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;
    
    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }
    
    [HttpPost(ApiRoutes.OrderActions.AddProductPath)]
    public async Task<IActionResult> AddProduct([FromBody]ProductCartRequest request)
    {
        var result = await _orderService.AddProductAsync(request);
        return result.Info is null ? BadRequest(result) : Ok(result);
    }
    
    [HttpDelete(ApiRoutes.OrderActions.DeleteProductPath)]
    public async Task<IActionResult> DeleteProduct([FromBody]ProductCartRequest request)
    {
        var result = await _orderService.DeleteProductAsync(request);
        return result.Info is null ? BadRequest(result) : Ok(result);
    }
    
    [HttpGet(ApiRoutes.OrderActions.GetOrdersPath)]
    public async Task<IActionResult> GetOrders([FromBody]Guid id)
    {
        var result = await _orderService.GetOrdersAsync(id);
        return result.ResponseObject is null ? BadRequest(result) : Ok(result);
    }
    
    [HttpGet(ApiRoutes.OrderActions.GetCartPath)]
    public async Task<IActionResult> GetCart([FromBody]Guid id)
    {
        var result = await _orderService.GetCartAsync(id);
        return result.ResponseObject is null ? BadRequest(result) : Ok(result);
    }
}