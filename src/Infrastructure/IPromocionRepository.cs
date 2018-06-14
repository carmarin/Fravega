using Promociones.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Promociones.Infrastructure
{
    public interface IPromocionRepository
    {
        Task<IEnumerable<Promocion>> ObtenerTodasPromociones();
        Task<IEnumerable<Promocion>> ObtenerPromocionFiltro(Promocion promoFiltro);
        Task<Promocion> ObtenerPromocion(Guid id);
        Task AgregarPromocion(Promocion item);
        Task<bool> RemoverPromocion(string id);
        Task<bool> RemoverTodasPromociones();
        Task<IEnumerable<Promocion>> ObtenerPromocionesPorEstado(bool activo);
        Task<IEnumerable<Promocion>> ObtenerPromocionesPorFecha(DateTime fecha);
        Task<IEnumerable<Promocion>> ObtenerPromocion(int idMedioPago, int idTipoMedioPago, int entidadFinanciera, int cantidadCuotas, int categoriaProducto, DateTime fecha);
        Task<bool> ActualizarPromocion(Promocion promocion);
        Task<long> ContarPromocionesAcopladas(int idMedioPago, DateTime fechaInicio, DateTime fechaFin);

    }
}
