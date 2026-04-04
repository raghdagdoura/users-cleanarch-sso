using Microsoft.EntityFrameworkCore;
using UsersApp.Domain.Entities;

namespace UsersApp.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public DbSet<User> Users => Set<User>();

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.FirstName).HasMaxLength(100).IsRequired();
            entity.Property(x => x.LastName).HasMaxLength(100).IsRequired();
            entity.Property(x => x.Email).HasMaxLength(200).IsRequired();
        });

        modelBuilder.Entity<User>().HasData(
            new User(Guid.Parse("11111111-1111-1111-1111-111111111111"), "Alice", "Martin", "alice@contoso.com"),
            new User(Guid.Parse("22222222-2222-2222-2222-222222222222"), "Bob", "Durand", "bob@contoso.com")
        );
    }
}