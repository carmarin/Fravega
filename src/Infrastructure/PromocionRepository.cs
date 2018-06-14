using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Promociones.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Promociones.Infrastructure
{
    public class PromocionRepository : IPromocionRepository
    {
        private readonly PromocionesContext _context = null;

        public PromocionRepository(IOptions<Settings> settings)
        {
            _context = new PromocionesContext(settings);
        }

        public async Task<IEnumerable<Promocion>> ObtenerTodasPromociones()
        {
            return await _context.Promociones.Find<Promocion>(_ => true).ToListAsync();
        }

        public async Task<Promocion> ObtenerPromocion(Guid id)
        {
            var filter = Builders<Promocion>.Filter.Eq("IdPromocion", id);

            return await _context.Promociones
                            .Find(filter)
                            .FirstOrDefaultAsync();

        }



        public async Task AgregarPromocion(Promocion item)
        {
            await _context.Promociones.InsertOneAsync(item);
        }

        public async Task<bool> RemoverPromocion(string id)
        {
            DeleteResult actionResult = await _context.Promociones.DeleteOneAsync(
                    Builders<Promocion>.Filter.Eq("IdPromocion", id));

            return actionResult.IsAcknowledged
                && actionResult.DeletedCount > 0;
        }

        public async Task<bool> RemoverTodasPromociones()
        {
            DeleteResult actionResult
                    = await _context.Promociones.DeleteManyAsync(new BsonDocument());

            return actionResult.IsAcknowledged
                && actionResult.DeletedCount > 0;
        }

        public async Task<IEnumerable<Promocion>> ObtenerPromocionFiltro(FilterDefinition<Promocion> promoFiltro)
        {
            return await _context.Promociones.Find<Promocion>(promoFiltro).ToListAsync();
        }

        public Task<IEnumerable<Promocion>> ObtenerPromocionFiltro(Promocion promoFiltro)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Promocion>> ObtenerPromocionesPorEstado(bool activo)
        {
            return await _context.Promociones.Find(Builders<Promocion>.Filter.Eq("Activo", activo)).ToListAsync();
        }

        public async Task<IEnumerable<Promocion>> ObtenerPromocionesPorFecha(DateTime fecha)
        {
            var filter = Builders<Promocion>.Filter.And(Builders<Promocion>.Filter.Lte("FechaInicio", fecha), Builders<Promocion>.Filter.Gte("FechaFin", fecha), Builders<Promocion>.Filter.Eq("Activo", true));
            return await _context.Promociones.Find(filter).ToListAsync();
        }

        public async Task<IEnumerable<Promocion>> ObtenerPromocion(int idMedioPago, int idTipoMedioPago, int entidadFinanciera, int cantidadCuotas, int categoriaProducto, DateTime fecha)
        {
            var filtroTipoMedioPago = Builders<Promocion>.Filter.Eq("TipoMedioPagoId", idTipoMedioPago);
            var filtroMedioPago = Builders<Promocion>.Filter.Eq("MedioPagoId", idMedioPago);
            var filtroEntidadFinanciera = Builders<Promocion>.Filter.Eq("EntidadFinancieraId", idMedioPago);
            var filtroCantidadCuotas = Builders<Promocion>.Filter.Or(Builders<Promocion>.Filter.Type("MaxCantidadDeCuotas", BsonType.Null), Builders<Promocion>.Filter.Lte("MaxCantidadDeCuotas", idMedioPago));
            var filtroCategoriaProducto = Builders<Promocion>.Filter.Eq("ProductoCategoriaIds", categoriaProducto);
            var filtroFecha = Builders<Promocion>.Filter.And(Builders<Promocion>.Filter.Gte("FechaInicio", fecha), Builders<Promocion>.Filter.Lte("FechaFin", fecha));
            var filtroEstado = Builders<Promocion>.Filter.Eq("Activo", true);
            var filter = Builders<Promocion>.Filter.And(filtroTipoMedioPago, filtroMedioPago, filtroEntidadFinanciera, filtroCantidadCuotas, filtroCategoriaProducto, filtroFecha, filtroEstado);
            return await _context.Promociones.Find(filter).ToListAsync();
        }

        public async Task<bool> ActualizarPromocion(Promocion promocion)
        {
            ReplaceOneResult actionResult = await _context.Promociones.ReplaceOneAsync(
                    Builders<Promocion>.Filter.Eq("IdPromocion", promocion.IdPromocion), promocion);

            return actionResult.IsAcknowledged
                && actionResult.ModifiedCount > 0;
        }
        

        public async Task<long> ContarPromocionesAcopladas(int idMedioPago, DateTime fechaInicio, DateTime fechaFin)
        {
            var filtroRangoInicial = Builders<Promocion>.Filter.And(Builders<Promocion>.Filter.Lte("FechaInicio", fechaInicio), Builders<Promocion>.Filter.Gt("FechaFin", fechaInicio));
            var filtroRangoFinal = Builders<Promocion>.Filter.And(Builders<Promocion>.Filter.Lt("FechaInicio", fechaFin), Builders<Promocion>.Filter.Gte("FechaFin", fechaFin));
            var filtroRangoIntermedio = Builders<Promocion>.Filter.And(Builders<Promocion>.Filter.Gte("FechaInicio", fechaInicio), Builders<Promocion>.Filter.Lte("FechaFin", fechaFin));
            var filtroTotal = Builders<Promocion>.Filter.Or(filtroRangoInicial, filtroRangoFinal, filtroRangoIntermedio);
            return await _context.Promociones.CountAsync(filtroTotal);
        }
    }
}