using System.ComponentModel.DataAnnotations;
using SharedLibrary.Models.Enums;

namespace SharedLibrary.Models.DtoModels.MainModels;

public class RoleDTO
{
    public int RoleDtoId { get; set; }
    
    public RoleTag RoleTag { get; set; }
    
    #region Constructors

    public RoleDTO(RoleTag roleTag)
    {
        RoleDtoId = (int)roleTag;
        
        RoleTag = roleTag;
    }
    
    #endregion
    
    #region Relationships
    
    //User
    public List<UserDTO> UsersDto { get; set; } = new ();
    
    #endregion
}