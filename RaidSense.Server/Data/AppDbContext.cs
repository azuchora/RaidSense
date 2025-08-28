using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RaidSense.Server.Models;

namespace RaidSense.Server.Data
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Map> Maps { get; set; }
        public DbSet<MapUser> MapUsers { get; set; }
        public DbSet<RustServer> Servers { get; set; }
        public DbSet<Base> Bases { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<BasePlayer> BasePlayers { get; set; }
        public DbSet<Photo> Photos { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<MapUser>()
                .HasOne(mu => mu.User)
                .WithMany(u => u.MapAccesses)
                .HasForeignKey(mu => mu.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<MapUser>()
                .HasOne(mu => mu.Map)
                .WithMany(m => m.MapUsers)
                .HasForeignKey(mu => mu.MapId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Map>()
                .HasOne(m => m.Owner)
                .WithMany(u => u.OwnedMaps)
                .HasForeignKey(m => m.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Map>()
               .HasOne(m => m.Server)
               .WithMany(s => s.Maps)
               .HasForeignKey(m => m.ServerId)
               .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Base>()
                .HasOne(b => b.Map)
                .WithMany(m => m.Bases)
                .HasForeignKey(b => b.MapId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Base>()
                .HasMany(b => b.Photos)
                .WithOne(p => p.Base)
                .HasForeignKey(p => p.BaseId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<BasePlayer>()
                .HasKey(bp => new { bp.BaseId, bp.PlayerId });

            builder.Entity<BasePlayer>()
                .HasOne(bp => bp.Base)
                .WithMany(b => b.BasePlayers)
                .HasForeignKey(bp => bp.BaseId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<BasePlayer>()
                .HasOne(bp => bp.Player)
                .WithMany(p => p.BasePlayers)
                .HasForeignKey(bp => bp.PlayerId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Photo>()
                .HasOne(p => p.Base)
                .WithMany(b => b.Photos)
                .HasForeignKey(p => p.BaseId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
