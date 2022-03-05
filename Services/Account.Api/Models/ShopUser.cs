using Microsoft.AspNetCore.Identity;

namespace Account.Api.Models
{
    public class ShopUser : IdentityUser
    {
        public string EmployeeNumber { get; set; }
    }
}
