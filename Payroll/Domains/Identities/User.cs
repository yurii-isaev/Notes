using Microsoft.AspNetCore.Identity;

namespace Payroll.Domains.Identities;

public class User : IdentityUser
{
    public DateTime Created { get; set; }
}
