using domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Laboratorio4.API.Context
{
    public class AplicacionDbContext : DbContext
    {
        public AplicacionDbContext(DbContextOptions<AplicacionDbContext> options) : base(options)
        {
        }

        public DbSet<HistoriaEntity> Historias { get; set; }
        public DbSet<PersonajeEntity> Personajes { get; set; }
        public DbSet<ControlEntity> Controles { get; set; }
        public DbSet<CreditoEntity> Creditos { get; set; }
        public DbSet<CreadorEntity> Creadores { get; set; }

    }
}
