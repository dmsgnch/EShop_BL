namespace SharedLibrary.Responses.Abstract;

public abstract class ResponseBase
{
    public string? ErrorInfo { get; set; }
    public string? Info { get; set; }
}