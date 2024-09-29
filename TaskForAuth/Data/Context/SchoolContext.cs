using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TaskForAuth.Data.Models;

namespace TaskForAuth.Data.Context
{
    public class SchoolContext : IdentityDbContext<SchoolUser>
    {
            public SchoolContext(DbContextOptions<SchoolContext> options) : base(options)
            {

            }
    }
}
