using InversionEnPozo.Movil.Modelos;
using System;
using System.Collections.Generic;
using System.Text;

namespace InversionEnPozo.Movil.Servicios.Interfaces
{
    public interface IProyectoService
    {
        Task<List<ProyectoInmobiliario>> ObtenerProyectosAsync();
    }
}
