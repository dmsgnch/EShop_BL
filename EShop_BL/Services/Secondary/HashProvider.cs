using System.Security.Cryptography;
using EShop_BL.Services.Secondary.Abstract;

namespace EShop_BL.Services.Secondary;

public class HashProvider : IHashProvider
{
    public string ComputeHash(string password, string saltString)
    {
        var salt = Convert.FromBase64String(saltString);
        byte[] bytes;

        using (var hashGenerator = new Rfc2898DeriveBytes(password, salt))
        {
            hashGenerator.IterationCount = 10101;
            bytes = hashGenerator.GetBytes(24);
        }
        return Convert.ToBase64String(bytes);
        
        // var salt = Convert.FromBase64String(saltString);
        // byte[] bytes;
        //
        // using (var hmac = new HMACSHA256(salt))
        // {
        //     bytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        // }
        // return Convert.ToBase64String(bytes);
    }

    public byte[] GenerateSalt()
    {
        var rng = RandomNumberGenerator.Create();
        var salt = new byte[24];
        rng.GetBytes(salt);
        return salt;
    }
}
