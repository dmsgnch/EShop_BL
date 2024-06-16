using SharedLibrary.Responses.Abstract;

namespace SharedLibrary.Responses;

public class LambdaResponse : ResponseBase
{
    public LambdaResponse()
    {
        
    }
    
    public LambdaResponse(string info)
    {
        Info = info;
    }
}