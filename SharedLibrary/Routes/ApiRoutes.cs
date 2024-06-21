namespace SharedLibrary.Routes;

public static class ApiRoutes
{
    public static class Controllers
    {
        public const string AuthenticationContr = "auth/";
        public const string UserContr = "user/";
        public const string SellerContr = "seller/";
        public const string ProductContr = "product/";
        public const string OrderContr = "order/";
        public const string OrderEventContr = "orderEvent/";
        public const string OrderItemContr = "orderItem/";
    }

    public static class AuthenticationActions
    {
        public const string RegisterPath = "reg";
        public const string LoginPath = "login";
    }
    
    public static class UserActions
    {
        public const string CreatePath = "create";
        public const string DeletePath = "delete/";
        public const string EditPath = "edit";
        public const string GetByIdPath = "GetById/";
        public const string GetAllPath = "GetAll";
    }
    
    public static class SellerActions
    {
        public const string CreatePath = "create";
        public const string DeletePath = "delete/";
        public const string EditPath = "edit";
        public const string GetByIdPath = "GetById/";
        public const string GetAllPath = "GetAll";
        public const string GetSellerIdByUserIdPath = "GetByUserId/";
    }
    
    public static class ProductActions
    {
        public const string CreatePath = "create";
        public const string DeletePath = "delete/";
        public const string EditPath = "edit";
        public const string GetByIdPath = "GetById/";
        public const string GetAllPath = "GetAll";
        public const string GetAllBySellerIdPath = "GetAllBySellerId";
    }
    
    public static class OrderActions
    {
        public const string GetOrdersPath = "GetOrders";
        public const string GetCartPath = "GetCart";
        public const string AddProductPath = "AddProduct";
        public const string DeleteProductPath = "DeleteProduct";
    }
    
    public static class OrderItemActions
    {
        public const string CreatePath = "create";
        public const string DeletePath = "delete/";
        public const string GetByIdPath = "GetById/";
        public const string GetAllPath = "GetAll";
    }
    
    public static class OrderEventActions
    {
        public const string CreatePath = "create";
        public const string DeletePath = "delete/";
        public const string GetByIdPath = "GetById/";
        public const string GetAllPath = "GetAll";
    }
}