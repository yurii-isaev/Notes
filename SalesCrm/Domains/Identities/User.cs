using Microsoft.AspNetCore.Identity;

namespace SalesCrm.Domains.Identities;

public class User : IdentityUser
{
    public DateTime Created { get; set; }
}
