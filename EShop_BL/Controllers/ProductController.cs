using EShop_BL.Services.Secondary.Abstract;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Models.ClientDtoModels.MainModels;
using SharedLibrary.Models.DtoModels.MainModels;
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
    
    [HttpPost(ApiRoutes.UniversalActions.CreateAction)]
    public async Task<IActionResult> CreateProduct([FromBody]ProductCDTO productCDto)
    {
        var result = await _productService.CreateProductAsync(productCDto);
        return result.ResponseObject is null ? BadRequest(result) : Ok(result);
    }
    
    [HttpDelete(ApiRoutes.UniversalActions.DeleteAction)]
    public async Task<IActionResult> DeleteProduct([FromBody]Guid id)
    {
        var result = await _productService.DeleteProductAsync(id);
        return result.Info is null ? BadRequest(result) : Ok(result);
    }
    
    [HttpPut(ApiRoutes.UniversalActions.EditAction)]
    public async Task<IActionResult> EditProduct([FromBody]ProductCDTO productCDto)
    {
        var result = await _productService.EditProductAsync(productCDto);
        return result.ResponseObject is null ? BadRequest(result) : Ok(result);
    }
    
    [HttpGet(ApiRoutes.UniversalActions.GetByIdAction)]
    public async Task<IActionResult> GetProductById([FromBody]Guid id)
    {
        var result = await _productService.GetProductByIdAsync(id);
        return result.ResponseObject is null ? BadRequest(result) : Ok(result);
    }
    
    [HttpGet(ApiRoutes.UniversalActions.GetAllAction)]
    public async Task<IActionResult> GetAllProducts()
    {
        var result = await _productService.GetAllProductsAsync();
        return result.ResponseObject is null ? BadRequest(result) : Ok(result);
    }
    
    [HttpGet(ApiRoutes.ProductActions.GetAllProductsBySellerIdAction)]
    public async Task<IActionResult> GetAllProductsBySellerId([FromBody]Guid id)
    {
        var result = await _productService.GetAllProductsBySellerIdAsync(id);
        return result.ResponseObject is null ? BadRequest(result) : Ok(result);
    }
}