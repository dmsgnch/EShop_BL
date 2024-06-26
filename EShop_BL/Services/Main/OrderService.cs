using EShop_BL.Components;
using EShop_BL.Extensions;
using EShop_BL.Helpers;
using EShop_BL.Services.Main.Abstract;
using Newtonsoft.Json;
using SharedLibrary.Models.ClientDtoModels.MainModels;
using SharedLibrary.Models.DtoModels.MainModels;
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

    public async Task<UniversalResponse> AddProductAsync(ProductCartRequest request)
    {
        if (request.CartId is null)
        {
            var orders = await _httpClient.SendRequestAsync(new RestRequestForm(
                endPoint: ApiRoutesDb.Controllers.UserContr + ApiRoutesDb.UniversalActions.GetByIdAction,
                requestMethod: HttpMethod.Get,
                jsonData: JsonConvert.SerializeObject(request.UserId)));

            var serverResponse = await JsonHelper.GetTypeFromResponseAsync<UniversalResponse<UserDTO>>(orders);

            if (!orders.IsSuccessStatusCode)
            {
                return new UniversalResponse(errorInfo: serverResponse.ErrorInfo);
            }

            if (serverResponse.ResponseObject is null) throw new Exception("User was not found");

            OrderDTO? cart =
                serverResponse.ResponseObject.OrdersDto?.FirstOrDefault(o =>
                    o.ProcessingStage.Equals(OrderProcessingStage.Cart));

            if (cart is null)
            {
                var responseCreateCart = await _httpClient.SendRequestAsync(new RestRequestForm(
                    endPoint: ApiRoutesDb.Controllers.OrderContr + ApiRoutesDb.OrderActions.CreateCartAction,
                    requestMethod: HttpMethod.Post,
                    jsonData: JsonConvert.SerializeObject(request.UserId)));

                var resultCreateCart =
                    await JsonHelper.GetTypeFromResponseAsync<UniversalResponse<OrderDTO>>(responseCreateCart);

                if (!responseCreateCart.IsSuccessStatusCode)
                {
                    return new UniversalResponse(errorInfo: resultCreateCart.ErrorInfo);
                }

                if (resultCreateCart.ResponseObject is null) throw new Exception("User was not found");
                request.CartId = resultCreateCart.ResponseObject.OrderDtoId;
            }
            else
            {
                request.CartId = cart.OrderDtoId;
            }
        }

        Guid cartId = request.CartId ?? throw new Exception("CartId is null");

        var response = await _httpClient.SendRequestAsync(new RestRequestForm(
            endPoint: ApiRoutesDb.Controllers.OrderItemContr + ApiRoutesDb.UniversalActions.CreateAction,
            requestMethod: HttpMethod.Post,
            jsonData: JsonConvert.SerializeObject(new OrderItemDTO(cartId, request.ProductId, 1))));

        return await EmptyResponseHandlerAsync(response);
    }

    public async Task<UniversalResponse> CreateOrderAsync(Guid userId)
    {
        var orders = await _httpClient.SendRequestAsync(new RestRequestForm(
            endPoint: ApiRoutesDb.Controllers.UserContr + ApiRoutesDb.UniversalActions.GetByIdAction,
            requestMethod: HttpMethod.Get,
            jsonData: JsonConvert.SerializeObject(userId)));

        var serverResponse = await JsonHelper.GetTypeFromResponseAsync<UniversalResponse<UserDTO>>(orders);

        if (!orders.IsSuccessStatusCode)
        {
            return new UniversalResponse(errorInfo: serverResponse.ErrorInfo);
        }

        if (serverResponse.ResponseObject is null) throw new Exception("User was not found");

        OrderDTO? cart =
            serverResponse.ResponseObject.OrdersDto?.FirstOrDefault(o =>
                o.ProcessingStage.Equals(OrderProcessingStage.Cart));

        if (cart is null) throw new Exception("Cart cant be null");

        var request = await _httpClient.SendRequestAsync(new RestRequestForm(
            endPoint: ApiRoutesDb.Controllers.OrderContr + ApiRoutesDb.OrderActions.CreateOrderAction,
            requestMethod: HttpMethod.Post,
            jsonData: JsonConvert.SerializeObject(cart.OrderDtoId)));

        return await EmptyResponseHandlerAsync(request);
    }

    public async Task<UniversalResponse> DeleteProductAsync(ProductCartRequest request)
    {
        if (request.CartId is null)
        {
            var orders = await _httpClient.SendRequestAsync(new RestRequestForm(
                endPoint: ApiRoutesDb.Controllers.UserContr + ApiRoutesDb.UniversalActions.GetByIdAction,
                requestMethod: HttpMethod.Get,
                jsonData: JsonConvert.SerializeObject(request.UserId)));

            var serverResponse = await JsonHelper.GetTypeFromResponseAsync<UniversalResponse<UserDTO>>(orders);

            if (!orders.IsSuccessStatusCode)
            {
                return new UniversalResponse(errorInfo: serverResponse.ErrorInfo);
            }

            if (serverResponse.ResponseObject is null) throw new Exception("User was not found");

            OrderDTO? cart =
                serverResponse.ResponseObject.OrdersDto?.FirstOrDefault(o =>
                    o.ProcessingStage.Equals(OrderProcessingStage.Cart));

            if (cart is null) throw new Exception("Cart is null");

            request.CartId = cart.OrderDtoId;
        }

        var orderItems = await _httpClient.SendRequestAsync(new RestRequestForm(
            endPoint: ApiRoutesDb.Controllers.OrderItemContr + ApiRoutesDb.UniversalActions.GetAllAction,
            requestMethod: HttpMethod.Get));

        var serverOrderItemsResponse =
            await JsonHelper.GetTypeFromResponseAsync<UniversalResponse<List<OrderItemDTO>>>(orderItems);

        if (!orderItems.IsSuccessStatusCode)
        {
            return new UniversalResponse(errorInfo: serverOrderItemsResponse.ErrorInfo);
        }

        if (serverOrderItemsResponse.ResponseObject is null) throw new Exception("User was not found");

        var orderItem = serverOrderItemsResponse.ResponseObject.FirstOrDefault(oi =>
            oi.OrderDtoId.Equals((Guid)request.CartId) && oi.ProductDtoId.Equals(request.ProductId));

        if (orderItem is null) throw new Exception("Order item was not found");

        var responseDeleteCart = await _httpClient.SendRequestAsync(new RestRequestForm(
            endPoint: ApiRoutesDb.Controllers.OrderItemContr + ApiRoutesDb.UniversalActions.DeleteAction,
            requestMethod: HttpMethod.Delete,
            jsonData: JsonConvert.SerializeObject(orderItem.OrderItemDtoId)));

        var resultDeleteCart = await JsonHelper.GetTypeFromResponseAsync<UniversalResponse>(responseDeleteCart);

        if (!responseDeleteCart.IsSuccessStatusCode)
        {
            return new UniversalResponse(errorInfo: resultDeleteCart.ErrorInfo);
        }

        return new UniversalResponse(info: resultDeleteCart.Info);
    }

    public async Task<UniversalResponse<OrderCDTO>> GetCartAsync(Guid id)
    {
        //Get user (+ role, seller, orders)
        var orders = await _httpClient.SendRequestAsync(new RestRequestForm(
            endPoint: ApiRoutesDb.Controllers.UserContr + ApiRoutesDb.UniversalActions.GetByIdAction,
            requestMethod: HttpMethod.Get,
            jsonData: JsonConvert.SerializeObject(id)));

        var serverResponse = await JsonHelper.GetTypeFromResponseAsync<UniversalResponse<UserDTO>>(orders);

        if (!orders.IsSuccessStatusCode)
        {
            return new UniversalResponse<OrderCDTO>(errorInfo: serverResponse.ErrorInfo);
        }

        if (serverResponse.ResponseObject is null) throw new Exception("User was not found");

        //Find cart order in user data
        OrderDTO? shortCart =
            serverResponse.ResponseObject.OrdersDto?.FirstOrDefault(o =>
                o.ProcessingStage.Equals(OrderProcessingStage.Cart));

        if (shortCart is null) return new UniversalResponse<OrderCDTO>(errorInfo: "Cart is empty");

        //Use found cart order id to get Order (+ OrderEvents, OrderItems (+ Product))
        var orderRequest = await _httpClient.SendRequestAsync(new RestRequestForm(
            endPoint: ApiRoutesDb.Controllers.OrderContr + ApiRoutesDb.UniversalActions.GetByIdAction,
            requestMethod: HttpMethod.Get,
            jsonData: JsonConvert.SerializeObject(shortCart.OrderDtoId)));

        var cart = await JsonHelper.GetTypeFromResponseAsync<UniversalResponse<OrderDTO>>(orderRequest);

        if (!orderRequest.IsSuccessStatusCode)
        {
            return new UniversalResponse<OrderCDTO>(errorInfo: cart.ErrorInfo);
        }

        if (cart.ResponseObject is null) throw new Exception("Order was not found");

        return new UniversalResponse<OrderCDTO>(responseObject: cart.ResponseObject.ToOrderCDto());
    }

    public async Task<UniversalResponse<List<OrderCDTO>>> GetOrdersAsync(Guid id)
    {
        //Get orders (+ OrderEvents, OrderItems (+ Product))
        var orders = await _httpClient.SendRequestAsync(new RestRequestForm(
            endPoint: ApiRoutesDb.Controllers.OrderContr + ApiRoutesDb.UniversalActions.GetAllAction,
            requestMethod: HttpMethod.Get));

        var serverResponse = await JsonHelper.GetTypeFromResponseAsync<UniversalResponse<List<OrderDTO>>>(orders);

        if (!orders.IsSuccessStatusCode)
        {
            return new UniversalResponse<List<OrderCDTO>>(errorInfo: serverResponse.ErrorInfo);
        }

        if (serverResponse.ResponseObject is null) throw new Exception("User was not found");

        var ordersResult = serverResponse.ResponseObject
            .Where(o => o.UserDtoId.Equals(id) && !o.ProcessingStage.Equals(OrderProcessingStage.Cart)).ToList();

        return new UniversalResponse<List<OrderCDTO>>(
            responseObject: ordersResult.Select(o => o.ToOrderCDto()).ToList());
    }

    private async Task<UniversalResponse> EmptyResponseHandlerAsync(HttpResponseMessage response)
    {
        var serverResponse = await JsonHelper.GetTypeFromResponseAsync<UniversalResponse>(response);

        if (!response.IsSuccessStatusCode)
        {
            return new UniversalResponse(errorInfo: serverResponse.ErrorInfo);
        }

        return new UniversalResponse(info: serverResponse.Info);
    }
}