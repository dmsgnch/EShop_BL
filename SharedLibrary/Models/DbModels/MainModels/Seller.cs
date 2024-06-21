using System.ComponentModel.DataAnnotations;

namespace SharedLibrary.Models.DbModels.MainModels;

public class Seller
{
    [Key]
    public Guid SellerId { get; set; } = Guid.NewGuid();
    
    #region Required parameters

    [Display(Name = "Name of company")]
    public string CompanyName { get; set; } = "";
    
    [Display(Name = "Contact number of the company")]
    public string ContactNumber { get; set; } = "";
    
    [Display(Name = "Company email")]
    public string EmailAddress { get; set; } = "";

    [Display(Name = "Company description")]
    public string CompanyDescription { get; set; }

    #endregion

    #region Optional parameters
    
    [Display(Name = "Company picture")]
    public string? ImageUrl { get; set; } = "";

    [Display(Name = "Addition contact number of the company")]
    public string? AdditionNumber { get; set; } = null;
    
    #endregion
    
    #region Constructors

    public Seller()
    { }

    public Seller(
        string companyName,
        string contactNumber,
        string emailAddress,
        string? imageUrl = null,
        string? companyDescription = null,
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
    
    #endregion

    #region Relationships

    //Product
    public List<Product>? Products { get; set; } = new();
    
    //User
    public ICollection<User>? Users { get; set; } = new List<User>();
    
    #endregion
}