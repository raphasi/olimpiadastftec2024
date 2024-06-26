using System;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Security.Principal;
using Microsoft.Extensions.Configuration;

namespace CRM.Application.Services;
public class LdapService
{
    private readonly IConfiguration _configuration;

    public LdapService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public bool ValidateUser(string email, string password)
    {
        var ldapSettings = _configuration.GetSection("LdapSettings");
        using (var context = new PrincipalContext(ContextType.Domain))
        {
            return context.ValidateCredentials(email, password);
        }
    }

    public bool IsUserInGroup(string email)
    {
        var ldapSettings = _configuration.GetSection("LdapSettings");
        using (var context = new PrincipalContext(ContextType.Domain))
        {
            UserPrincipal userPrincipal = new UserPrincipal(context);
            userPrincipal.UserPrincipalName = email;

            using (var user = UserPrincipal.FindByIdentity(context, IdentityType.UserPrincipalName, email))
            {
                if (user != null)
                {
                    using (var group = GroupPrincipal.FindByIdentity(context, ldapSettings["GroupDn"]))
                    {
                        return group != null && user.IsMemberOf(group);
                    }
                }
            }
        }
        return false;
    }

    public string GetUserObjectId(string email)
    {
        var ldapSettings = _configuration.GetSection("LdapSettings");
        using (var context = new PrincipalContext(ContextType.Domain))
        {
            using (var user = UserPrincipal.FindByIdentity(context, email))
            {
                return user?.Sid.ToString();
            }
        }
    }
}