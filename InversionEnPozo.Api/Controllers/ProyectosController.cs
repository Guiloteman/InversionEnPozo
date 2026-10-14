using InversionEnPozo.Api.Modelos;
using InversionEnPozo.Api.Servicios.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InversionEnPozo.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProyectosController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProyectosController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerProyectos()
        {
            var proyectos = await _unitOfWork.Proyectos.ObtenerTodosAsync();
            return Ok(proyectos);
        }

        [HttpPost]
        public async Task<IActionResult> CrearProyecto([FromBody] ProyectoInversion proyecto)
        {
            await _unitOfWork.Proyectos.AgregarAsync(proyecto);
            await _unitOfWork.CompleteAsync();

            return CreatedAtAction(nameof(ObtenerProyectos), new { id = proyecto.Id }, proyecto);
        }
    }
}