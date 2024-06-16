namespace SharedLibrary.Requests;

public class GetUserByTokenRequest
{
    public string Token { get; set; }

    public GetUserByTokenRequest()
    {
        
    }

    public GetUserByTokenRequest(string token)
    {
        Token = token;
    }
}