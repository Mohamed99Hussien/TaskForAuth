using Microsoft.AspNetCore.Identity;

namespace TaskForAuth.Data.Models
{
    public class SchoolUser : IdentityUser
    {
        public string SchoolName { get; set; }
        public int PerformanceRate { get; set; }
    }
}
