using Microsoft.AspNetCore.Identity;

namespace API.DAL.Models;

public class Role : IdentityRole<Guid>, IBaseEntity
{
    public DateTime CreateDateTime { get; set; }
    public DateTime ModifyDateTime { get; set; }
    public DateTime? DeleteDateTime { get; set; }
}