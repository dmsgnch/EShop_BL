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
        public const string CreatePath = "add";
        public const string DeleteControllerPath = "delete/{id}";
        public const string DeletePath = "delete/";
        public const string UpdatePath = "update";
        public const string GetAllPath = "getAll";
        public const string GetByIdControllerPath = "GetById/{id}";
        public const string GetByIdPath = "GetById/";

    }
    
    public static class OrderActions
    {
        public const string CreatePath = "Create/";
        public const string AddOrderItemPath = "AddOrderItem/";
        public const string CreateCartPath = "CreateCart/";
        public const string CreateOrderPath = "CreateOrder/";
        public const string GetOrderByIdPath = "GetOrderById/";
    }
}