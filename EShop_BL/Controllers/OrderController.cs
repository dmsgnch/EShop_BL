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
    
    [HttpPost(ApiRoutes.OrderActions.AddProductToCartOrderAction)]
    public async Task<IActionResult> AddProductToCartOrder([FromBody]ProductCartRequest request)
    {
        var result = await _orderService.AddProductAsync(request);
        return result.Info is null ? BadRequest(result) : Ok(result);
    }
    
    [HttpPost(ApiRoutes.OrderActions.CreateOrder)]
    public async Task<IActionResult> CreateOrder([FromBody]Guid userId)
    {
        var result = await _orderService.CreateOrderAsync(userId);
        return result.Info is null ? BadRequest(result) : Ok(result);
    }
    
    [HttpDelete(ApiRoutes.OrderActions.DeleteProductFromCartOrderAction)]
    public async Task<IActionResult> DeleteProductFromCartOrder([FromBody]ProductCartRequest request)
    {
        var result = await _orderService.DeleteProductAsync(request);
        return result.Info is null ? BadRequest(result) : Ok(result);
    }
    
    [HttpGet(ApiRoutes.OrderActions.GetAllOrdersNotCartAction)]
    public async Task<IActionResult> GetOrdersNotCart([FromBody]Guid id)
    {
        var result = await _orderService.GetOrdersAsync(id);
        return result.ResponseObject is null ? BadRequest(result) : Ok(result);
    }
    
    [HttpGet(ApiRoutes.OrderActions.GetCartOrderAction)]
    public async Task<IActionResult> GetCartOrder([FromBody]Guid id)
    {
        var result = await _orderService.GetCartAsync(id);
        return result.ResponseObject is null ? BadRequest(result) : Ok(result);
    }
}