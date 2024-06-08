namespace EShop_BL.Services.Abstract;

public interface IHashProvider
{
    string ComputeHash(string password, string saltString);
    byte[] GenerateSalt();
}
