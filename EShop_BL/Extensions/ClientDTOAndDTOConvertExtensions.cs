using SharedLibrary.Models.ClientDtoModels.MainModels;
using SharedLibrary.Models.ClientDtoModels.SecondaryModels;
using SharedLibrary.Models.DtoModels.MainModels;
using SharedLibrary.Models.DtoModels.SecondaryModels;
using SharedLibrary.Requests;

namespace EShop_BL.Extensions;

public static class ClientDTOAndDTOConvertExtensions
{
    #region RegisterRequest
    
    public static UserDTO ToUserDto(this RegisterRequest registerRequest)
    {
        return new UserDTO(
            registerRequest.Name,
            registerRequest.LastName,
            registerRequest.PhoneNumber,
            registerRequest.Email,
            patronymic: registerRequest.Patronymic,
            passwordHash: registerRequest.Password
        );
    }
    
    #endregion
    
    #region User
    
    public static UserCDTO ToUserCDto(this UserDTO userDto)
    {
        return new UserCDTO(
            userDto.Name,
            userDto.LastName,
            userDto.PhoneNumber,
            userDto.Email,
            userDto.RoleDtoId,
            patronymic: userDto.Patronymic
        )
        {
            UserCDtoId = userDto.UserDtoId,
          
            RoleCDto = userDto.RoleDto?.ToRoleCDto(),
            
            SellerCDtoId = userDto.SellerDtoId,
            SellerCDto = userDto.SellerDto?.ToSellerCDto(), 
            
            OrdersCDto = userDto.OrdersDto?.Select(o => o.ToOrderCDto()).ToList(),
                
            RecipientCDto = userDto.RecipientDto?.ToRecipientCDto(),
            DeliveryAddressCDto = userDto.DeliveryAddressDto?.ToDeliveryAddressCDto(),
        };
    }
    public static UserDTO ToUserDto(this UserCDTO userCDto)
    {
        return new UserDTO(
            userCDto.Name,
            userCDto.LastName,
            userCDto.PhoneNumber,
            userCDto.Email,
            userCDto.Patronymic
        )
        {
            UserDtoId = userCDto.UserCDtoId,
            
            RoleDtoId = userCDto.RoleCDtoId,
            RoleDto = userCDto.RoleCDto?.ToRoleDto(),
            
            SellerDtoId = userCDto.SellerCDtoId,
            SellerDto = userCDto.SellerCDto?.ToSellerDto(),
            
            OrdersDto = userCDto.OrdersCDto?.Select(o => o.ToOrderDto()).ToList(),
            
            RecipientDto = userCDto.RecipientCDto?.ToRecipientDto(),
            DeliveryAddressDto = userCDto.DeliveryAddressCDto?.ToDeliveryAddressDto()
        };
    }
    
    #endregion
    
    #region Order
    
    public static OrderCDTO ToOrderCDto(this OrderDTO orderDto)
    {
        return new OrderCDTO(orderDto.UserDtoId, orderDto.AnonymousToken)
        {
            OrderCDtoId = orderDto.OrderDtoId,
            
            UserCDto = orderDto.UserDto?.ToUserCDto(),
            
            OrderEventsCDto = orderDto.OrderEventsDto?.Select(oi => oi.ToOrderEventCDto()).ToList(),
            OrderItemsCDto = orderDto.OrderItemsDto?.Select(oi => oi.ToOrderItemCDto()).ToList(),
            
            RecipientCDto = orderDto.RecipientDto?.ToRecipientCDto(),
            DeliveryAddressCDto = orderDto.DeliveryAddressDto?.ToDeliveryAddressCDto()
        };
    }
    public static OrderDTO ToOrderDto(this OrderCDTO orderCDto)
    {
        return new OrderDTO()
        {
            OrderDtoId = orderCDto.OrderCDtoId,
            
            AnonymousToken = orderCDto.AnonymousToken,
            
            UserDtoId = orderCDto.UserCDtoId,
            UserDto = orderCDto.UserCDto?.ToUserDto(),
            
            OrderEventsDto = orderCDto.OrderEventsCDto?.Select(oe => oe.ToOrderEventDto()).ToList(),
            OrderItemsDto = orderCDto.OrderItemsCDto?.Select(oi => oi.ToOrderItemDto()).ToList(),
            
            RecipientDto = orderCDto.RecipientCDto?.ToRecipientDto(),
            DeliveryAddressDto = orderCDto.DeliveryAddressCDto?.ToDeliveryAddressDto()
        };
    }
    
    #endregion
    
    #region OrderItem
    
    public static OrderItemCDTO ToOrderItemCDto(this OrderItemDTO orderItemDto)
    {
        return new OrderItemCDTO(orderItemDto.OrderDtoId, orderItemDto.ProductDtoId, orderItemDto.Quantity)
        {
            OrderItemCDtoId = orderItemDto.OrderItemDtoId,
            
            OrderCDto = orderItemDto.OrderDto?.ToOrderCDto(),
            ProductCDto = orderItemDto.ProductDto?.ToProductCDto(),
        };
    }
    public static OrderItemDTO ToOrderItemDto(this OrderItemCDTO orderItemCDto)
    {
        return new OrderItemDTO(orderItemCDto.OrderCDtoId, orderItemCDto.ProductCDtoId, orderItemCDto.Quantity)
        {
            OrderItemDtoId = orderItemCDto.OrderItemCDtoId,
            
            OrderDto = orderItemCDto.OrderCDto?.ToOrderDto(),
            ProductDto = orderItemCDto.ProductCDto?.ToProductDto(),
        };
    }
    
    #endregion
    
    #region OrderEvent
    
    public static OrderEventCDTO ToOrderEventCDto(this OrderEventDTO orderEventDto)
    {
        return new OrderEventCDTO(orderEventDto.OrderDtoId, orderEventDto.Stage)
        {
            OrderEventCDtoId = orderEventDto.OrderEventDtoId,
            
            EventTime = orderEventDto.EventTime,
            OrderCDto = orderEventDto.OrderDto?.ToOrderCDto(),
        };
    }
    public static OrderEventDTO ToOrderEventDto(this OrderEventCDTO orderEventCDto)
    {
        return new OrderEventDTO(orderEventCDto.OrderCDtoId, orderEventCDto.Stage)
        {
            OrderEventDtoId = orderEventCDto.OrderEventCDtoId,
            
            EventTime = orderEventCDto.EventTime,
            OrderDto = orderEventCDto.OrderCDto?.ToOrderDto(),
        };
    }
    
    #endregion
    
    #region Product
    
    public static ProductCDTO ToProductCDto(this ProductDTO productDto)
    {
        return new ProductCDTO(
            productDto.Name,
            productDto.Description,
            productDto.PricePerUnit,
            productDto.WeightInGrams,
            productDto.SellerDtoId,
            productDto.ImageUrl)
        {
            ProductCDtoId = productDto.ProductDtoId,
            
            InStock = productDto.InStock,
            
            SellerCDto = productDto.SellerDto?.ToSellerCDto(),
            
            OrderItemsCDto = productDto.OrderItemsDto?.Select(oi => oi.ToOrderItemCDto()).ToList(),
        };
    }
    public static ProductDTO ToProductDto(this ProductCDTO productCDto)
    {
        return new ProductDTO(
            productCDto.Name,
            productCDto.Description,
            productCDto.PricePerUnit,
            productCDto.WeightInGrams,
            productCDto.SellerCDtoId,
            productCDto.ImageUrl)
        {
            ProductDtoId = productCDto.ProductCDtoId,
            
            InStock = productCDto.InStock,
            
            SellerDto = productCDto.SellerCDto?.ToSellerDto(),
            
            OrderItemsDto = productCDto.OrderItemsCDto?.Select(oi => oi.ToOrderItemDto()).ToList(),
        };
    }
    
    #endregion
    
    #region Role
    
    public static RoleCDTO ToRoleCDto(this RoleDTO roleDto)
    {
        return new RoleCDTO(roleDto.RoleTag)
        {
            RoleCDtoId = roleDto.RoleDtoId,
            
            UsersCDto = roleDto.UsersDto?.Select(u => u.ToUserCDto()).ToList() ?? new(),
        };
    }
    public static RoleDTO ToRoleDto(this RoleCDTO roleCDto)
    {
        return new RoleDTO(roleCDto.RoleTag)
        {
            RoleDtoId = roleCDto.RoleCDtoId,
            
            UsersDto = roleCDto.UsersCDto?.Select(u => u.ToUserDto()).ToList() ?? new(),
        };
    }
    
    #endregion
    
    #region Seller
    
    public static SellerCDTO ToSellerCDto(this SellerDTO sellerDto)
    {
        return new SellerCDTO(
            sellerDto.CompanyName,
            sellerDto.ContactNumber,
            sellerDto.EmailAddress,
            sellerDto.CompanyDescription,
            sellerDto.ImageUrl,
            sellerDto.AdditionNumber)
        {
            SellerCDtoId = sellerDto.SellerDtoId,
            
            ProductsCDto = sellerDto.ProductsDto?.Select(pr => pr.ToProductCDto()).ToList(),
            UsersCDto = sellerDto.UsersDto?.Select(u => u.ToUserCDto()).ToList(),
        };
    }
    public static SellerDTO ToSellerDto(this SellerCDTO sellerCDto)
    {
        return new SellerDTO(
            sellerCDto.CompanyName,
            sellerCDto.ContactNumber,
            sellerCDto.EmailAddress,
            sellerCDto.CompanyDescription,
            sellerCDto.ImageUrl,
            sellerCDto.AdditionNumber)
        {
            SellerDtoId = sellerCDto.SellerCDtoId,
            
            ProductsDto = sellerCDto.ProductsCDto?.Select(pr => pr.ToProductDto()).ToList(),
            UsersDto = sellerCDto.UsersCDto?.Select(u => u.ToUserDto()).ToList(),
        };
    }
    
    #endregion
    
    #region DeliveryAddress
    
    public static DeliveryAddressCDTO ToDeliveryAddressCDto(this DeliveryAddressDTO deliveryAddress)
    {
        return new DeliveryAddressCDTO(
            deliveryAddress.City,
            deliveryAddress.Street,
            deliveryAddress.House,
            deliveryAddress.Apartment,
            deliveryAddress.Floor,
            deliveryAddress.UserDtoId,
            deliveryAddress.OrderDtoId)
        {
            DeliveryAddressCDtoId = deliveryAddress.DeliveryAddressDtoId,
            
            UserCDto = deliveryAddress.UserDto?.ToUserCDto(),
            OrderCDto = deliveryAddress.OrderDto?.ToOrderCDto(),
        };
    }
    public static DeliveryAddressDTO ToDeliveryAddressDto(this DeliveryAddressCDTO deliveryAddressCDto)
    {
        return new DeliveryAddressDTO(
            deliveryAddressCDto.City,
            deliveryAddressCDto.Street,
            deliveryAddressCDto.House,
            deliveryAddressCDto.Apartment,
            deliveryAddressCDto.Floor,
            deliveryAddressCDto.UserCDtoId,
            deliveryAddressCDto.OrderCDtoId)
        {
            DeliveryAddressDtoId = deliveryAddressCDto.DeliveryAddressCDtoId,
            
            UserDto = deliveryAddressCDto.UserCDto?.ToUserDto(),
            OrderDto = deliveryAddressCDto.OrderCDto?.ToOrderDto(),
        };
    }
    
    #endregion
    
    #region Recipient
    
    public static RecipientCDTO ToRecipientCDto(this RecipientDTO recipientDto)
    {
        return new RecipientCDTO(
            recipientDto.Name,
            recipientDto.LastName,
            recipientDto.PhoneNumber,
            recipientDto.Patronymic,
            recipientDto.UserDtoId,
            recipientDto.OrderDtoId)
        {
            RecipientCDtoId = recipientDto.RecipientDtoId,
            
            UserCDto = recipientDto.UserDto?.ToUserCDto(),
            OrderCDto = recipientDto.OrderDto?.ToOrderCDto(),
        };
    }
    public static RecipientDTO ToRecipientDto(this RecipientCDTO recipientCDto)
    {
        return new RecipientDTO(
            recipientCDto.Name,
            recipientCDto.LastName,
            recipientCDto.PhoneNumber,
            recipientCDto.Patronymic,
            recipientCDto.UserCDtoId,
            recipientCDto.OrderCDtoId)
        {
            RecipientDtoId= recipientCDto.RecipientCDtoId,
            
            UserDto = recipientCDto.UserCDto?.ToUserDto(),
            OrderDto = recipientCDto.OrderCDto?.ToOrderDto(),
        };
    }
    
    #endregion
}