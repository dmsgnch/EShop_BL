using SharedLibrary.Models.MainModels;
using SharedLibrary.Responses.Abstract;

namespace SharedLibrary.Responses;

public class GetUserResponse : ResponseBase
{
    public User? User { get; set; }
    
    public GetUserResponse()
    { }

    public GetUserResponse(User? user = null, string? info = null)
    {
        User = user;
        Info = info;
    }
}