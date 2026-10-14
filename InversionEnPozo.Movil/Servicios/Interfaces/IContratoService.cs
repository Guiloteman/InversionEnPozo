using InversionEnPozo.Movil.Modelos;
using System;
using System.Collections.Generic;
using System.Text;

namespace InversionEnPozo.Movil.Servicios.Interfaces
{
    public interface IContratoService
    {
        Task<(bool Exito, CrearProyectoResponseDto? Datos, string Mensaje)> CrearProyectoAsync(string nombre, string descripcion, decimal presupuesto, string constructoraId);
        Task<(bool Exito, AprobarHitoResponseDto? Datos, string Mensaje)> AprobarHitoAsync(int hitoId);
        Task<List<ProyectoResponseDto>> ObtenerProyectosActivosAsync();
    }
}
