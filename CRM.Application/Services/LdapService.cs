using System;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
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
        using (var context = new PrincipalContext(ContextType.Domain, ldapSettings["Server"], ldapSettings["UserDn"], ldapSettings["Password"]))
        {
            return context.ValidateCredentials(email, password);
        }
    }

    public bool IsUserInGroup(string email)
    {
        var ldapSettings = _configuration.GetSection("LdapSettings");
        using (var context = new PrincipalContext(ContextType.Domain, ldapSettings["Server"], ldapSettings["UserDn"], ldapSettings["Password"]))
        {
            using (var user = UserPrincipal.FindByIdentity(context, email))
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
        using (var context = new PrincipalContext(ContextType.Domain, ldapSettings["Server"], ldapSettings["UserDn"], ldapSettings["Password"]))
        {
            using (var user = UserPrincipal.FindByIdentity(context, email))
            {
                return user?.Sid.ToString();
            }
        }
    }
}