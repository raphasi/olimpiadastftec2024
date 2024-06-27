using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Application.DTOs
{
    public class UserInfoDTO
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public IList<string>? Roles { get; set; }
        public Guid? LeadID { get; set; }
        public Guid? UserID { get; set; }
        public Guid? ObjectID { get; set; }
        public string? SecurityIdentifier { get; set; }
    }
}
