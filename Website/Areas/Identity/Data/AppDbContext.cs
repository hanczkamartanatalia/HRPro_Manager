using Database.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using Website.Areas.Identity.Data;
using Website.Entities;


namespace Website.Data;

public class AppDbContext : IdentityDbContext<AppUser, IdentityRole<int>, int>
{
    public DbSet<Employment> Employments{ get; set; }
    public DbSet<Application> Applications { get; set; }
    public DbSet<Position> Positions { get; set; }
    public DbSet<WorkTime> WorkTimes { get; set; }
    public DbSet<LoginData> LoginData { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Employment>()
                .Property(e => e.Rate)
                .HasColumnType("money");

        modelBuilder.Entity<LoginData>()
            .HasOne(ul => ul.User)
            .WithMany()
            .HasForeignKey(ul => ul.Id_User)
            .OnDelete(DeleteBehavior.NoAction); // lub .OnDelete(DeleteBehavior.Restrict)


    }
}
