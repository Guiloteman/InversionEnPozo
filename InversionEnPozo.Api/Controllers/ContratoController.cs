using InversionEnPozo.Api.DTOs;
using InversionEnPozo.Api.Hubs;
using InversionEnPozo.Api.Modelos;
using InversionEnPozo.Api.Servicios;
using InversionEnPozo.Api.Servicios.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace InversionEnPozo.Api.Controllers
{
    
    [Authorize(Roles = "Constructora")]
    [Route("api/[controller]")]
    [ApiController]
    public class ContratoController : ControllerBase
    {
        private readonly IEthereumSmartContractService _smartContractService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHubContext<NotificacionHub> _hubContext;

        public ContratoController(IEthereumSmartContractService smartContractService, IUnitOfWork unitOfWork, IHubContext<NotificacionHub> hubContext)
        {
            _smartContractService = smartContractService;
            _unitOfWork = unitOfWork;
            _hubContext = hubContext;
        }

        [HttpPost("crear-proyecto")]
        public async Task<IActionResult> CrearProyecto([FromBody] CrearProyectoDto model)
        {
            string hashGenesis = await _smartContractService.RegistrarProyectoConContratoAsync(model.NombreProyecto, model.PresupuestoTotal);

            var nuevoProyecto = new ProyectoInversion
            {
                Titulo = model.NombreProyecto,
                Descripcion = model.Descripcion,
                CostoTotal = model.PresupuestoTotal,
                MontoRecaudado = 0,
                ConstructoraId = model.ConstructoraId
            };
            await _unitOfWork.Proyectos.AgregarAsync(nuevoProyecto);
            await _unitOfWork.CompleteAsync();

            return Ok(new
            {
                mensaje = "Proyecto guardado en Base de Datos y transacción enviada a la Blockchain con éxito.",
                proyectoId = nuevoProyecto.Id,
                hashContratoGenesis = hashGenesis
            });
        }

        [HttpPost("aprobar-hito/{hitoId:int}")]
        public async Task<IActionResult> AprobarHito(int hitoId)
        {
            var resultado = await _smartContractService.AprobarHitoAsync(hitoId);

            if (!resultado.Exito)
            {
                return BadRequest(new { mensaje = resultado.Mensaje });
            }

            return Ok(new   
            {
                mensaje = resultado.Mensaje,
                hitoId = hitoId,
                hashTransaccionHito = resultado.HashBloque,
                fechaLiberacion = DateTime.UtcNow
            });
        }

        [HttpGet("proyectos")]
        public async Task<IActionResult> ObtenerProyectos()
        {
            var proyectos = await _unitOfWork.Proyectos.ObtenerTodosAsync();

            var listaDto = proyectos.Select(p => new
            {
                p.Id,
                p.Titulo,
                p.Descripcion,
                p.CostoTotal,
                p.MontoRecaudado,
                Porcentaje = p.CostoTotal > 0 ? (p.MontoRecaudado / p.CostoTotal) * 100 : 0
            });

            return Ok(listaDto);
        }

        [HttpPost("solicitar-desembolso")]
        public async Task<IActionResult> SolicitarDesembolso([FromBody] SolicitarDesembolsoDto model)
        {
            // 1. Validar el hito en el contrato inteligente mediante Nethereum (Sepolia)
            var resultado = await _smartContractService.AprobarHitoAsync(model.HitoId);

            if (!resultado.Exito)
            {
                return BadRequest(new { mensaje = resultado.Mensaje });
            }

            // 2. Actualizar el estado en la Base de Datos (SQL Server con EF Core)
            var hitoDb = await _unitOfWork.Hitos.ObtenerPorIdAsync(model.HitoId);
            if (hitoDb != null)
            {
                hitoDb.Estado = "AprobadoLiberado";
                hitoDb.HashTransaccion = resultado.HashBloque;
                hitoDb.FechaActualizacion = DateTime.UtcNow;
                _unitOfWork.Hitos.Actualizar(hitoDb);
                await _unitOfWork.CompleteAsync();
            }

            await _hubContext.Clients.All.SendAsync("RecibirNotificacionDesembolso", new
            {
                mensaje = $"¡Hito #{model.HitoId} aprobado y fondos liberados en Sepolia!",
                hash = resultado.HashBloque,
                fecha = DateTime.UtcNow
            });

            return Ok(new
            {
                mensaje = "Desembolso de hito solicitado y procesado en la blockchain con éxito.",
                hitoId = model.HitoId,
                hashTransaccionHito = resultado.HashBloque,
                fechaLiberacion = DateTime.UtcNow
            });
        }
    }
}