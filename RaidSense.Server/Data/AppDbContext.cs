using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RaidSense.Server.Models;

namespace RaidSense.Server.Data
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<RustMap> Maps { get; set; }
        public DbSet<UserMap> UserMaps { get; set; }
        public DbSet<MapUser> MapUsers { get; set; }
        public DbSet<RustServer> Servers { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<MapUser>()
                .HasOne(mu => mu.User)
                .WithMany(u => u.MapAccesses)
                .HasForeignKey(mu => mu.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<MapUser>()
                .HasOne(mu => mu.Map)
                .WithMany(um => um.MapUsers)
                .HasForeignKey(mu => mu.MapId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<MapUser>()
                .HasIndex(mu => new { mu.MapId, mu.UserId })
                .IsUnique();

            builder.Entity<UserMap>()
                .HasOne(um => um.Map)
                .WithMany(m => m.UserMaps)
                .HasForeignKey(um => um.MapId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<UserMap>()
                .HasOne(um => um.Owner)
                .WithMany(u => u.OwnedMaps)
                .HasForeignKey(um => um.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<RustServer>()
                .HasOne(rs => rs.Map)
                .WithMany(m => m.Servers)
                .HasForeignKey(rs => rs.MapId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<RefreshToken>()
                .HasOne(rt => rt.User)
                .WithMany(u => u.RefreshTokens)
                .HasForeignKey(rt => rt.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
