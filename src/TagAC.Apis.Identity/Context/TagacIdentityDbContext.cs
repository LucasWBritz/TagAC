using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TagAC.Apis.Identity.Context
{
    public class TagacIdentityDbContext : IdentityDbContext
    {
        public TagacIdentityDbContext(DbContextOptions<TagacIdentityDbContext> options) : base(options) { }
    }
}
