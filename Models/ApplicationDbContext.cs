using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.Data.Entity;
using ColorWars.ViewModels.Couleur;

namespace ColorWars.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<ApplicationUser> AppUser { get; set; }
        public DbSet<Couleur> Couleurs { get; set; }
        public DbSet<Squad> Squads { get; set; }
        public DbSet<Bataille> Batailles { get; set; }
        public DbSet<Tour> Tours { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //
            // ----- Couleurs
            //

            builder.Entity<Couleur>()
                .HasOne(c => c.SquadCourante)
                .WithMany(s => s.Couleurs)
                .HasForeignKey(c => c.SquadCouranteId);

            builder.Entity<Couleur>()
                .HasOne(c => c.User)
                .WithMany(au => au.Couleurs);

            //
            // ----- Squads
            //

            builder.Entity<Squad>()
                .HasOne(s => s.User)
                .WithMany(au => au.Squads);

            builder.Entity<Squad>()
                .HasMany(s => s.Couleurs)
                .WithOne(c => c.SquadCourante);

            //
            // ----- Batailles
            //

            builder.Entity<Bataille>()
                .HasMany(b => b.Tours)
                .WithOne(t => t.Bataille);

            builder.Entity<Bataille>()
                .HasMany(b => b.BattleSets)
                .WithOne(ps => ps.Bataille);

            //
            // ----- BattleSet
            //

            builder.Entity<BattleSet>()
                .HasKey(bs => new {bs.UserId, bs.BatailleId});

            builder.Entity<BattleSet>()
                .HasOne(bs => bs.Bataille)
                .WithMany(b => b.BattleSets)
                .HasForeignKey(bs => bs.BatailleId);

            builder.Entity<BattleSet>()
                .HasOne(bs => bs.Squad)
                .WithOne();

            //
            // ----- Tours
            //

            //builder.Entity<Tour>()
            //    .HasIndex(t => t.NumDansBataille);

            builder.Entity<Tour>()
                .HasOne(t => t.Bataille)
                .WithMany(b => b.Tours)
                .HasForeignKey(t => t.BatailleId);

            // TODO: les actions sur le builder par le Fluid API.
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

    }
}