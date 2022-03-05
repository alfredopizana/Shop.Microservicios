using Account.Api.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Account.Api.Context
{
    public class AccountDbContext : IdentityDbContext<ShopUser>
    {
        public AccountDbContext(DbContextOptions options) : base(options)
        {

        }
    }
}
