
namespace EShop_BL.Components
{
    public class HttpRequestForm
    {
        public string EndPoint { get; }
        public HttpMethod RequestMethod { get; }
        public string? Token { get; set; }
        public string? JsonData { get; }

        public HttpRequestForm(string endPoint,
            HttpMethod requestMethod,
            string token = null,
            string jsonData = null)
        {
            EndPoint = endPoint;
            RequestMethod = requestMethod;
            Token = token;
            JsonData = jsonData;
        }
    }
}