
using Devella.DataAccessLayer.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Devella.DataAccessLayer.Data;

public class ApplicationDbContext : IdentityDbContext<User>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {

    }

    public DbSet<CompanyUser> CompanyUsers { get; set; } = null!;
    public DbSet<DeveloperUser> DeveloperUsers { get; set; } = null!;
    public DbSet<AdminUser> AdminUsers { get; set; } = null!;
}
