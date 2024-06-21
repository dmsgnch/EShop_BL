using EShop_BL.Services.Main.Abstract;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Requests;
using SharedLibrary.Routes;

namespace EShop_BL.Controllers;

[ApiController]
[Route(ApiRoutes.Controllers.ProductContr)]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;
    
    public ProductController(IProductService productService)
    {
        _productService = productService;
    }
    
    [HttpPost(ApiRoutes.ProductActions.CreatePath)]
    public async Task<IActionResult> CreateProduct([FromBody]EditProductRequest request)
    {
        var result = await _productService.CreateProductAsync(request);
        return result.ResponseObject is null ? BadRequest(result) : Ok(result);
    }
    
    [HttpDelete(ApiRoutes.ProductActions.DeletePath)]
    public async Task<IActionResult> DeleteProduct([FromBody]Guid id)
    {
        var result = await _productService.DeleteProductAsync(id);
        return result.Info is null ? BadRequest(result) : Ok(result);
    }
    
    [HttpPut(ApiRoutes.ProductActions.EditPath)]
    public async Task<IActionResult> EditProduct([FromBody]EditProductRequest request)
    {
        var result = await _productService.EditProductAsync(request);
        return result.ResponseObject is null ? BadRequest(result) : Ok(result);
    }
    
    [HttpGet(ApiRoutes.ProductActions.GetByIdPath)]
    public async Task<IActionResult> GetProductById([FromBody]Guid id)
    {
        var result = await _productService.GetProductByIdAsync(id);
        return result.ResponseObject is null ? BadRequest(result) : Ok(result);
    }
    
    [HttpGet(ApiRoutes.ProductActions.GetAllPath)]
    public async Task<IActionResult> GetAllProducts()
    {
        var result = await _productService.GetAllProductsAsync();
        return result.ResponseObject is null ? BadRequest(result) : Ok(result);
    }
    
    [HttpGet(ApiRoutes.ProductActions.GetAllBySellerIdPath)]
    public async Task<IActionResult> GetAllProductsBySellerId([FromBody]Guid id)
    {
        var result = await _productService.GetAllProductsBySellerIdAsync(id);
        return result.ResponseObject is null ? BadRequest(result) : Ok(result);
    }
}