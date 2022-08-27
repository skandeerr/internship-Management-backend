using interships_Management.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace interships_Management.Data;

public class ContextDb : IdentityDbContext<ApplicationUser>  {
    public DbSet<ProjectModel> Projects => Set<ProjectModel>();
    public DbSet<EntretienModel> Entretiens => Set<EntretienModel>();
    public DbSet<PostProject> PostProjects => Set<PostProject>();


public ContextDb(DbContextOptions<ContextDb> options): base(options)
    {


    }
    protected override void OnModelCreating(ModelBuilder modelBuilder) { base.OnModelCreating(modelBuilder); modelBuilder.Entity<IdentityUser>().Ignore(c => c.UserName)
    ; }

}