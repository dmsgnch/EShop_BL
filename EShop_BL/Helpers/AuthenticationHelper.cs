using EShop_BL.Models.MainModels;
using EShop_BL.Services.Abstract;

namespace EShop_BL.Helpers;

public static class AuthenticationHelper
{
    public static void ProvideSaltAndHash(this User user, IHashProvider hashProvider)
    {
        var salt = hashProvider.GenerateSalt();
        user.Salt = Convert.ToBase64String(salt);
        user.PasswordHash = hashProvider.ComputeHash(user.PasswordHash, user.Salt);
    }
}