using EShop_BL.Components;

namespace EShop_BL.Services.Abstract;

public interface IHttpClientService
{
    public Task<HttpResponseMessage> SendRequestAsync(RestRequestForm requestForm);
}