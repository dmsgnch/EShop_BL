using System.ComponentModel.DataAnnotations;
using SharedLibrary.Models.Enums;

namespace SharedLibrary.Models.DtoModels.MainModels;

public class RoleDTO
{
    public int RoleDtoId { get; set; }
    
    public RoleTag RoleTag { get; set; }

    public RoleDTO(RoleTag roleTag)
    {
        RoleDtoId = (int)roleTag;
        
        RoleTag = roleTag;
    }
    
    #region Relationships
    
    //User
    public List<UserDTO> UsersDto { get; set; } = new ();
    
    #endregion
}