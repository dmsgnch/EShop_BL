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

public class LambdaResponse<T> : ResponseBase where T: class
{
    public T? ResponseObject { get; set; }

    public LambdaResponse()
    {
        
    }
    
    public LambdaResponse(string info = null, T responseObject = null)
    {
        Info = info;
        ResponseObject = responseObject;
    }
}