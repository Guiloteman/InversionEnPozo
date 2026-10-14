using System.Linq.Expressions;

namespace InversionEnPozo.Api.Repositorios.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> ObtenerTodosAsync();
        Task<T?> ObtenerPorIdAsync(int id);
        Task<IEnumerable<T>> BuscarAsync(Expression<Func<T, bool>> predicado);
        Task AgregarAsync(T entidad);
        void Actualizar(T entidad);
        void Eliminar(T entidad);
    }
}
