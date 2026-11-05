using domain.dto;
namespace Laboratorio4.API.Services.Interface
{
    public interface ILaboratorioService
    {
        public Task<HistoriaDto> GetHistoriaAsync();
        public Task<IEnumerable<PersonajeDto>> GetPersonajesAsync(string? tipo);
        public Task<PersonajeDto> GetPersonajeByIdAsync(int id);
        public Task<IEnumerable<ControlDto>> GetControlesAsync();
        public Task<CreditoDto> GetCreditosAsync();
    }
}
