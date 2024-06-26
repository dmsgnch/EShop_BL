using System.ComponentModel.DataAnnotations;
using SharedLibrary.Models.Enums;

namespace SharedLibrary.Models.ClientDtoModels.MainModels;

public class RoleCDTO
{
    public int RoleCDtoId { get; set; }
    
    public RoleTag RoleTag { get; set; }
    

    public RoleCDTO(RoleTag roleTag)
    {
        RoleCDtoId = (int)roleTag;
        
        RoleTag = roleTag;
    }

    #region Relationships
    
    //User
    public List<UserCDTO> UsersCDto { get; set; } = new ();
    
    #endregion
}