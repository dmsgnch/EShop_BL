namespace EShop_BL.Services.Abstract;

public interface ISellerService
{
    Task<HttpResponseMessage> GetDbResponseGetAllSellersAsync();
}