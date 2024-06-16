namespace EShop_BL.Services.Main.Abstract;

public interface ISellerService
{
    Task<HttpResponseMessage> GetDbResponseGetAllSellersAsync();
}