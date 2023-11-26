using Microsoft.EntityFrameworkCore;
using Website.Entities;
using Website.Models.Account;

namespace Website.Database;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Employment> Employments { get; set; }
    public DbSet<Application> Applications { get; set; }
    public DbSet<Position> Positions { get; set; }
    public DbSet<WorkTime> WorkTimes { get; set; }
    public DbSet<LoginData> LoginData { get; set; }
    


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=tcp:hrpro-server.database.windows.net,1433;Initial Catalog=HRPro;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;Authentication='Active Directory Default';");
        base.OnConfiguring(optionsBuilder);

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Employment>()
                .Property(e => e.Rate)
                .HasColumnType("money");

        modelBuilder.Entity<LoginData>()
                .HasIndex(ld => ld.Login)
                .IsUnique();

    }
}
