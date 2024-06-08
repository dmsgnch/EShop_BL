using EShop_BL.Models.MainModels;

namespace SharedLibrary.Responses.Abstract;

public interface IAuthenticationResponse
{
    public User User { get; set; }
    public string? Token { get; set; }
}