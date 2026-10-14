using InversionEnPozo.Api.Datos.Interfaces;
using InversionEnPozo.Api.Modelos;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace InversionEnPozo.Api.Datos
{
    public class AppDbContext : IdentityDbContext, IAppDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<ProyectoInversion> ProyectosInversiones { get; set; }
        public DbSet<Inversion> Inversiones { get; set; }
        public DbSet<HitoObra> HitosObras { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ProyectoInversion>()
                .HasMany(p => p.Hitos)
                .WithOne(h => h.ProyectoInversion)
                .HasForeignKey(h => h.ProyectoInversionId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProyectoInversion>()
                .HasMany(p => p.Inversiones)
                .WithOne(i => i.ProyectoInversion)
                .HasForeignKey(i => i.ProyectoInversionId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}