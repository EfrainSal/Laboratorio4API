using Laboratorio4.API.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Laboratorio4.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LaboratorioController : Controller
    {
        private readonly ILaboratorioService _laboratorioService;

        public LaboratorioController(ILaboratorioService laboratorioService)
        {
            _laboratorioService = laboratorioService;
        }

        [HttpGet("historia")]
        public async Task<IActionResult> GetHistoria()
        {
            var resultado = await _laboratorioService.GetHistoriaAsync();
            return Ok(resultado);
        }

        [HttpGet("personajes")]
        public async Task<IActionResult> GetPersonajes([FromQuery] string? tipo)
        {
            var resultado = await _laboratorioService.GetPersonajesAsync(tipo);
            return Ok(resultado);
        }

        [HttpGet("personajes/{id}")]
        public async Task<IActionResult> GetPersonajeById(int id)
        {
            var resultado = await _laboratorioService.GetPersonajeByIdAsync(id);
            if (resultado == null) return NotFound(new { mensaje = "Personaje no encontrado" });
            return Ok(resultado);
        }

        [HttpGet("controles")]
        public async Task<IActionResult> GetControles()
        {
            var resultado = await _laboratorioService.GetControlesAsync();
            return Ok(resultado);
        }

        [HttpGet("creditos")]
        public async Task<IActionResult> GetCreditos()
        {
            var resultado = await _laboratorioService.GetCreditosAsync();
            return Ok(resultado);
        }
    }
}
