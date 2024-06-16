namespace SharedLibrary.Routes;

public static class ApiRoutesDb
{
    public static class Controllers
    {
        public const string DeliveryAddress = "deliveryAddress/";
        public const string Order = "order/";
        public const string OrderEvent = "orderEvent/";
        public const string OrderItem = "orderItem/";
        public const string Product = "product/";
        public const string Recipient = "recipient/";
        public const string Role = "role/";
        public const string Seller = "seller/";
        public const string User = "user/";
    }
    
    public static class Universal
    {
        public const string Create = "add";
        public const string DeleteController = "delete/{id}";
        public const string Delete = "delete/";
        public const string Update = "update";
        public const string GetAll = "getAll";
        public const string GetByIdController = "GetById/{id}";
        public const string GetById = "GetById/";
        
    }
}