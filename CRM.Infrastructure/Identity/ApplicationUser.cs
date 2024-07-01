using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Infrastructure.Identity;

public class ApplicationUser : IdentityUser
{
    public string? FullName { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }
    public Guid? UserId { get; set; }
    public Guid? LeadID { get; set; }
    public Guid? ObjectID { get; set; }
    // Propriedade para armazenar o SecurityIdentifier como string
    public string? SecurityIdentifierString { get; set; }

    // Propriedade não mapeada para manipular o SecurityIdentifier
    [NotMapped]
    public SecurityIdentifier SecurityIdentifier
    {
        get => new SecurityIdentifier(SecurityIdentifierString);
        set => SecurityIdentifierString = value?.Value;
    }
}
