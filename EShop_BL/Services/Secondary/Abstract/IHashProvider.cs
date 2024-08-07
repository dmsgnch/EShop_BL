namespace EShop_BL.Services.Secondary.Abstract;

public interface IHashProvider
{
    string ComputeHash(string password, string saltString);
    byte[] GenerateSalt();
}
