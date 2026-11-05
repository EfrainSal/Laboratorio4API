using domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Laboratorio4.API.Context
{
    public class AplicacionDbContext : DbContext
    {
        public AplicacionDbContext(DbContextOptions<AplicacionDbContext> options) : base(options)
        {
        }

        public DbSet<RankingEntity> Rankings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RankingEntity>()
                .HasIndex(r => new { r.Score, r.Time, r.Id })
                .IsDescending(true, false, false);
        }

        public DbSet<HistoriaEntity> Historias { get; set; }
        public DbSet<PersonajeEntity> Personajes { get; set; }
        public DbSet<ControlEntity> Controles { get; set; }
        public DbSet<CreditoEntity> Creditos { get; set; }
        public DbSet<CreadorEntity> Creadores { get; set; }

    }
}
