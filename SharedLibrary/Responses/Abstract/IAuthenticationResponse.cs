using SharedLibrary.Models.DtoModels.MainModels;

namespace SharedLibrary.Responses.Abstract;

public interface IAuthenticationResponse
{
    public string? Token { get; set; }
}