using Microsoft.AspNetCore.Identity;

namespace API.DAL.Models
{
    public class User : IdentityUser<Guid>, IBaseEntity
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime ModifyDateTime { get; set; }
        public DateTime? DeleteDateTime { get; set; }
    }
}
