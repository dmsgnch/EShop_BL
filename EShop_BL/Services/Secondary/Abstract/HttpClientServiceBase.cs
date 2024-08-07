using System.Net;
using EShop_BL.Components;
using Newtonsoft.Json;
using SharedLibrary.Responses;

namespace EShop_BL.Services.Secondary.Abstract;

public abstract class HttpClientServiceBase
{
    public async Task<HttpResponseMessage> ProcessRequestAsync(HttpRequestForm requestForm)
    {
        try
        {
            var request = CreateHttpRequest(requestForm);
            var response = await SendHttpRequestAsync(request);
            return await HandleResponseAsync(response);
        }
        catch (HttpRequestException httpEx)
        {
            await LogMachine.LogRequestToJson($"HTTP request error: {httpEx.Message}");
            Console.WriteLine($"HTTP request error: {httpEx.Message}");
            return new HttpResponseMessage(HttpStatusCode.BadGateway)
            {
                Content = new StringContent(
                    JsonConvert.SerializeObject(new UniversalResponse(
                        "An unexpected error occurred while trying to access the server. Please try again! " +
                        "If the problem persists, please contact support.")))
            };
        }
        catch (TaskCanceledException taskEx)
        {
            await LogMachine.LogRequestToJson("Request was canceled (possibly due to timeout).");
            Console.WriteLine("Request was canceled (possibly due to timeout).");
            return new HttpResponseMessage(HttpStatusCode.BadGateway)
            {
                Content = new StringContent(
                    JsonConvert.SerializeObject(new UniversalResponse("The server's not responding")))
            };
        }
        catch (Exception ex)
        {
            await LogMachine.LogRequestToJson($"Unexpected error: {ex.Message}");
            Console.WriteLine($"Unexpected error: {ex.Message}");
            throw new Exception($"An unexpected error occurred: {ex.Message}");
        }
    }

    protected abstract HttpRequestMessage CreateHttpRequest(HttpRequestForm requestForm);
    protected abstract Task<HttpResponseMessage> SendHttpRequestAsync(HttpRequestMessage request);
    protected abstract Task<HttpResponseMessage> HandleResponseAsync(HttpResponseMessage response);
}