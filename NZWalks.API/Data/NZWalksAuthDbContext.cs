using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace NZWalks.API.Data;

public class NZWalksAuthDbContext : IdentityDbContext
{
    public NZWalksAuthDbContext(DbContextOptions<NZWalksAuthDbContext> options) : base(options)
    {

    }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Seed data for Roles
        var readerRoleId = "856aaed1-f859-40cf-ba7f-416a207b08c0";
        var writerRoleId = "03af2f66-8ef2-4fd6-a4ba-c92bbda596af";

        var roles = new List<IdentityRole>
        {
            new IdentityRole
            {
                Id = readerRoleId,
                ConcurrencyStamp = readerRoleId,
                Name = "Reader",
                NormalizedName = "Reader".ToUpper()
            },
            new IdentityRole
            {
                Id = writerRoleId,
                ConcurrencyStamp = writerRoleId,
                Name = "Writer",
                NormalizedName = "Writer".ToUpper()
            }
        };
        builder.Entity<IdentityRole>().HasData(roles);
    
    }
}
