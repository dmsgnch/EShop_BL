using System.ComponentModel.DataAnnotations;

namespace SharedLibrary.Models.DtoModels.MainModels;

public class SellerDTO
{
    public Guid SellerDtoId { get; set; } = Guid.NewGuid();
    
    public string CompanyName { get; set; }
    public string ContactNumber { get; set; }
    public string EmailAddress { get; set; }
    public string CompanyDescription { get; set; }

    public string? ImageUrl { get; set; }
    public string? AdditionNumber { get; set; }

    public SellerDTO(
        string companyName,
        string contactNumber,
        string emailAddress,
        string companyDescription,
        string? imageUrl = null,
        string? additionNumber = null
    )
    {
        CompanyName = companyName;
        ContactNumber = contactNumber;
        EmailAddress = emailAddress;
        ImageUrl = imageUrl;
        CompanyDescription = companyDescription;
        AdditionNumber = additionNumber;
    }

    #region Relationships

    //Product
    public List<ProductDTO>? ProductsDto { get; set; } = new();
    
    //User
    public List<UserDTO>? UsersDto { get; set; } = new();
    
    #endregion
}