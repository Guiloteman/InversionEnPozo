namespace InversionEnPozo.Api.Servicios.Interfaces
{
    public interface IEthereumSmartContractService
    {
        Task<string> RegistrarProyectoConContratoAsync(string nombreProyecto, decimal presupuesto);
        Task<(bool Exito, string Mensaje, string HashBloque)> AprobarHitoAsync(int hitoId);
    }
}
