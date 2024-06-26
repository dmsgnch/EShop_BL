using SharedLibrary.Responses.Abstract;

namespace SharedLibrary.Responses;

public class UniversalResponse : ResponseBase
{
    public UniversalResponse()
    {
        
    }
    
    public UniversalResponse(string? errorInfo = null, string? info = null)
    {
        Info = info;
        ErrorInfo = errorInfo;
    }
}

public class UniversalResponse<T> : ResponseBase where T: class
{
    public T? ResponseObject { get; set; }

    public UniversalResponse()
    {
        
    }
    
    public UniversalResponse(string? errorInfo = null, string? info = null, T? responseObject = null)
    {
        Info = info;
        ErrorInfo = errorInfo;
        ResponseObject = responseObject;
    }
}