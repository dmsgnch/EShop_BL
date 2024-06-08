using EShop_BL.Components;
using EShop_BL.Services.Abstract;

namespace EShop_BL.Services;

public class SellerService : ISellerService
{
    private readonly HttpClientService _httpClientService;

    public SellerService(HttpClientService httpClientService)
    {
        _httpClientService = httpClientService;
    }

    public async Task<HttpResponseMessage> GetDbResponseGetAllSellersAsync()
    {
        var requestForm = new RestRequestForm("seller/getAll", HttpMethod.Get);

        return await _httpClientService.SendRequestAsync(requestForm);
    }
}