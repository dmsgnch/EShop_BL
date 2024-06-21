using EShop_BL.Components;
using EShop_BL.Helpers;
using EShop_BL.Services.Main.Abstract;
using Newtonsoft.Json;
using SharedLibrary.Models.DbModels.MainModels;
using SharedLibrary.Models.Enums;
using SharedLibrary.Requests;
using SharedLibrary.Responses;
using SharedLibrary.Routes;

namespace EShop_BL.Services.Main;

public class OrderService : IOrderService
{
    private readonly HttpClientService _httpClient;

    public OrderService(HttpClientService httpClientService)
    {
        _httpClient = httpClientService;
    }

    public async Task<LambdaResponse> AddProductAsync(ProductCartRequest request)
    {
        var orders = await _httpClient.SendRequestAsync(new RestRequestForm(
            ApiRoutesDb.Controllers.UserContr + ApiRoutesDb.UniversalActions.GetByIdPath + request.UserId,
            HttpMethod.Get));

        var serverResponse = await JsonHelper.GetTypeFromResponseAsync<LambdaResponse<User>>(orders);

        if (!orders.IsSuccessStatusCode)
        {
            return new LambdaResponse(errorInfo: serverResponse.ErrorInfo);
        }

        User? user = serverResponse.ResponseObject;

        if (user is null) throw new Exception("User was not found");

        Order? cart = user.Orders.FirstOrDefault(o => o.ProcessingStage.Equals(OrderProcessingStage.Cart));

        if (cart is null)
        {
            //Create new Cart
        }

        request.CartId = cart.OrderId;
        
        var response = await _httpClient.SendRequestAsync(new RestRequestForm(
            ApiRoutesDb.Controllers.OrderContr + ApiRoutesDb.OrderActions.AddOrderItemPath,
            HttpMethod.Post, jsonData: JsonConvert.SerializeObject(request)));

        return await EmptyResponseHandlerAsync(response);
    }

    public async Task<LambdaResponse> DeleteProductAsync(ProductCartRequest request)
    {
        return new LambdaResponse();
    }

    public async Task<LambdaResponse<Order>> GetCartAsync(Guid id)
    {
        var userRequest = await _httpClient.SendRequestAsync(new RestRequestForm(
            ApiRoutesDb.Controllers.UserContr + ApiRoutesDb.UniversalActions.GetByIdPath + id,
            HttpMethod.Get));

        var userResponse = await JsonHelper.GetTypeFromResponseAsync<LambdaResponse<User>>(userRequest);

        if (!userRequest.IsSuccessStatusCode)
        {
            return new LambdaResponse<Order>(errorInfo: userResponse.ErrorInfo);
        }

        if (userResponse.ResponseObject is null) throw new Exception("User was not found");

        var orderResult = userResponse.ResponseObject.Orders.FirstOrDefault(r => r.ProcessingStage == OrderProcessingStage.Cart);

        if (orderResult is null) throw new Exception("result is null");
        
        var orderRequest = await _httpClient.SendRequestAsync(new RestRequestForm(
            ApiRoutesDb.Controllers.OrderContr + ApiRoutesDb.UniversalActions.GetByIdPath + orderResult.OrderId,
            HttpMethod.Get));

        var cart = await JsonHelper.GetTypeFromResponseAsync<LambdaResponse<Order>>(userRequest);

        if (!orderRequest.IsSuccessStatusCode)
        {
            return new LambdaResponse<Order>(errorInfo: cart.ErrorInfo);
        }

        if (cart.ResponseObject is null) throw new Exception("Order was not found");

        return new LambdaResponse<Order>(responseObject: cart.ResponseObject);
    }

    public async Task<LambdaResponse<List<Order>>> GetOrdersAsync(Guid id)
    {
        var userRequest = await _httpClient.SendRequestAsync(new RestRequestForm(
            ApiRoutesDb.Controllers.UserContr + ApiRoutesDb.UniversalActions.GetByIdPath + id,
            HttpMethod.Get));

        var userResponse = await JsonHelper.GetTypeFromResponseAsync<LambdaResponse<User>>(userRequest);

        if (!userRequest.IsSuccessStatusCode)
        {
            return new LambdaResponse<List<Order>>(errorInfo: userResponse.ErrorInfo);
        }

        if (userResponse.ResponseObject is null) throw new Exception("User was not found");

        var ordersResult = userResponse.ResponseObject.Orders.Where(r => r.ProcessingStage != OrderProcessingStage.Cart).ToList();

        if (ordersResult is null) throw new Exception("results is null");

        return new LambdaResponse<List<Order>>(responseObject: ordersResult);
    }

    private async Task<LambdaResponse> EmptyResponseHandlerAsync(HttpResponseMessage response)
    {
        var serverResponse = await JsonHelper.GetTypeFromResponseAsync<LambdaResponse>(response);

        if (!response.IsSuccessStatusCode)
        {
            return new LambdaResponse(errorInfo: serverResponse.ErrorInfo);
        }

        return new LambdaResponse(info: serverResponse.Info);
    }
}