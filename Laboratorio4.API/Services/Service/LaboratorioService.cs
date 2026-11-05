using domain.dto;
using Laboratorio4.API.Context;
using Laboratorio4.API.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace Laboratorio4.API.Services.Service
{
    public class LaboratorioService : ILaboratorioService
    {
        private readonly AplicacionDbContext _context;

        public LaboratorioService(AplicacionDbContext context)
        {
            _context = context;
        }
        public async Task<HistoriaDto?> GetHistoriaAsync()
        {
            var historia = await _context.Historias.FirstOrDefaultAsync();
            if (historia == null) { return null; }

            return new HistoriaDto
            {
                Titulo = historia.Titulo,
                Subtitulo = historia.Subtitulo,
                Contenido = historia.Contenido
            };
        }

        public async Task<IEnumerable<PersonajeDto>> GetPersonajesAsync(string? tipo)
        {
            var query = _context.Personajes.AsQueryable();

            if (!string.IsNullOrEmpty(tipo))
            {
                query = query.Where(p => p.Rol == tipo);
            }

            return await query.Select(p => new PersonajeDto
            {
                Id = p.Id,
                Nombre = p.Nombre,
                Rol = p.Rol,
                Genero = p.Genero,
                Vestimenta = p.Vestimenta,
                Habilidad = p.Habilidad,
                Biografia = p.Biografia,
                ImagenUrl = p.ImagenUrl
            }).ToListAsync();
        }

        public async Task<PersonajeDto?> GetPersonajeByIdAsync(int id)
        {
            var p = await _context.Personajes.FindAsync(id);
            if (p == null) return null;

            return new PersonajeDto
            {
                Id = p.Id,
                Nombre = p.Nombre,
                Rol = p.Rol,
                Genero = p.Genero,
                Vestimenta = p.Vestimenta,
                Habilidad = p.Habilidad,
                Biografia = p.Biografia,
                ImagenUrl = p.ImagenUrl
            };
        }

        public async Task<IEnumerable<ControlDto>> GetControlesAsync()
        {
            return await _context.Controles.Select(c => new ControlDto
            {
                Tecla = c.Tecla,
                Accion = c.Accion
            }).ToListAsync();
        }

        public async Task<CreditoDto> GetCreditosAsync()
        {
            var infoCredito = await _context.Creditos.FirstOrDefaultAsync();
            var listaCreadores = await _context.Creadores.Select(c => c.Nombre).ToListAsync();

            return new CreditoDto
            {
                Grupo = infoCredito?.Grupo ?? "Sin grupo asignado",
                Descripcion = infoCredito?.Descripcion ?? "Sin descripción",
                Creadores = listaCreadores
            };
        }

    }
}
