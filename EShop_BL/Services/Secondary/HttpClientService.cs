using System.Net;
using System.Net.Http.Headers;
using System.Text;
using EShop_BL.Components;
using EShop_BL.Services.Secondary.Abstract;
using Newtonsoft.Json;
using SharedLibrary.Responses;

namespace EShop_BL.Services.Secondary;

public class HttpClientService : HttpClientServiceBase
{
    private readonly IHttpClientFactory _clientFactory;
    private const string DbUrl = "https://localhost:44381/";

    public HttpClientService(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }

    protected override HttpRequestMessage CreateHttpRequest(HttpRequestForm requestForm)
    {
        var request = new HttpRequestMessage(requestForm.RequestMethod, DbUrl + requestForm.EndPoint);

        if (requestForm.JsonData != null)
        {
            request.Content = new StringContent(requestForm.JsonData, Encoding.UTF8, "application/json");
        }

        if (!string.IsNullOrWhiteSpace(requestForm.Token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", requestForm.Token);
        }

        return request;
    }

    protected override async Task<HttpResponseMessage> SendHttpRequestAsync(HttpRequestMessage request)
    {
        var client = _clientFactory.CreateClient("MyHttpClient");
        var response = await client.SendAsync(request);
        return response;
    }

    protected override async Task<HttpResponseMessage> HandleResponseAsync(HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode)
        {
            await LogMachine.LogRequestToJson("Successful communication with the database server");
            return response;
        }
        else
        {
            return new HttpResponseMessage(response.StatusCode)
            {
                Content = new StringContent(
                    JsonConvert.SerializeObject(new UniversalResponse(
                        "Api service error. Please try again!")))
            };
        }
    }
}