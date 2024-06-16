namespace EShop_BL.Services.Main.Abstract;

public interface IHashProvider
{
    string ComputeHash(string password, string saltString);
    byte[] GenerateSalt();
}
