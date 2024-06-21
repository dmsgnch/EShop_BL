using SharedLibrary.Models.DbModels.MainModels;
using SharedLibrary.Models.DtoModels.MainModels;
using SharedLibrary.Responses.Abstract;

namespace SharedLibrary.Responses;

public class GetUserResponse : ResponseBase
{
    public User? User { get; set; }
    
    public GetUserResponse()
    { }

    public GetUserResponse(User? user = null, string? errorInfo = null)
    {
        User = user;
        ErrorInfo = errorInfo;
    }
}