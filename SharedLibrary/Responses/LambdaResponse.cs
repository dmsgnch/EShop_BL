using SharedLibrary.Responses.Abstract;

namespace SharedLibrary.Responses;

public class LambdaResponse : ResponseBase
{
    public LambdaResponse()
    {
        
    }
    
    public LambdaResponse(string? errorInfo = null, string? info = null)
    {
        Info = info;
        ErrorInfo = errorInfo;
    }
}

public class LambdaResponse<T> : ResponseBase where T: class
{
    public T? ResponseObject { get; set; }

    public LambdaResponse()
    {
        
    }
    
    public LambdaResponse(string? errorInfo = null, string? info = null, T? responseObject = null)
    {
        Info = info;
        ErrorInfo = errorInfo;
        ResponseObject = responseObject;
    }
}