namespace SharedLibrary.Routes;

public static class ApiRoutesDb
{
    public static class Controllers
    {
        public const string DeliveryAddressContr = "deliveryAddress/";
        public const string OrderContr = "order/";
        public const string OrderEventContr = "orderEvent/";
        public const string OrderItemContr = "orderItem/";
        public const string ProductContr = "product/";
        public const string RecipientContr = "recipient/";
        public const string RoleContr = "role/";
        public const string SellerContr = "seller/";
        public const string UserContr = "user/";
    }
    
    public static class UniversalActions
    {
        public const string CreateAction = "add";
        public const string DeleteAction = "delete/";
        public const string UpdateAction = "update";
        public const string GetAllAction = "getAll";
        public const string GetByIdAction = "GetById/";

    }
    
    public static class OrderActions
    {
        public const string CreateCartAction = "CreateCartOrder/";
        public const string CreateOrderAction = "CreateOrder/";
    }
}