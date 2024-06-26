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
        public const string RegisterAction = "reg";
        public const string LoginAction = "login";
    }

    public static class UniversalActions
    {
        public const string CreateAction = "create";
        public const string DeleteAction = "delete/";
        public const string EditAction = "edit";
        public const string GetByIdAction = "GetById/";
        public const string GetAllAction = "GetAll";
    }

    public static class ProductActions
    {
        public const string GetAllProductsBySellerIdAction = "GetAllBySellerId";
    }
    
    public static class OrderActions
    {
        public const string GetAllOrdersNotCartAction = "GetOrders";
        public const string CreateOrder = "CreateOrder";
        public const string GetCartOrderAction = "GetCart";
        public const string AddProductToCartOrderAction = "AddProduct";
        public const string DeleteProductFromCartOrderAction = "DeleteProduct";
    }

    public static class SellerActions
    {
        public const string GetSellerIdByUserIdAction = "GetSellerIdByUserId";
    }
}