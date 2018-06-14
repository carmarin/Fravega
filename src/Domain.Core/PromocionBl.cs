using HttpUtils;
using Microsoft.Extensions.Configuration;
using Promociones.Domain.Entities;
using Promociones.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Promociones.Domain.Core
{
    public class PromocionBl
    {
        private readonly IPromocionRepository _repositorio = null;
        private readonly string _rutaServicioMedio = string.Empty;
        private readonly string _rutaServicioCategoria = string.Empty;
        public PromocionBl(IPromocionRepository repositorio, IConfiguration config)
        {
            _rutaServicioMedio = config["UrlApiMedio"];
            _rutaServicioCategoria = config["UrlApiCategoria"];
            _repositorio = repositorio;
        }

        public async Task<IEnumerable<Promocion>> ObtenerTodasPromociones()
        {
            return await _repositorio.ObtenerTodasPromociones();
        }

        public async Task<IEnumerable<Promocion>> ObtenerPromocionesVigentes()
        {
            return await _repositorio.ObtenerPromocionesPorEstado(true);
        }

        public async Task<IEnumerable<Promocion>> ObtenerPromocionesPorFecha(DateTime fecha)
        {
            return await _repositorio.ObtenerPromocionesPorFecha(fecha);
        }

        public async Task<Promocion> ObtenerPromocion(Guid idPromocion)
        {
            return await _repositorio.ObtenerPromocion(idPromocion);
        }

        public async Task<IEnumerable<Promocion>> ObtenerPromocionesVigentes(int idMedioPago, int idTipoMedioPago, int entidadFinanciera, int cantidadCuotas, int categoriaProducto)
        {
            return await _repositorio.ObtenerPromocion(idMedioPago, idTipoMedioPago, entidadFinanciera, cantidadCuotas, categoriaProducto, DateTime.Now);
        }

        public async Task AgregarPromocion(Promocion promocion)
        {
            await _repositorio.AgregarPromocion(promocion);
        }

        public async Task EliminarPromociones()
        {
            await _repositorio.RemoverTodasPromociones();
        }

        public async Task ModificarPromocion(Guid idPromocion, int[] idMedioPago, int[] idTipoMedioPago, int[] entidadFinanciera, int? cantidadCuotas, int[] categoriaProducto, int porcentajeDescuento)
        {
            var promocion =  await _repositorio.ObtenerPromocion(idPromocion);
            if (promocion == null)
            {
                throw new NullReferenceException();
            }
            promocion.MedioPagoId = idMedioPago;
            promocion.TipoMedioPagoId = idTipoMedioPago;
            promocion.EntidadFinancieraId = entidadFinanciera;
            promocion.MaxCantidadDeCuotas = cantidadCuotas;
            promocion.ProductoCategoriaIds = categoriaProducto;
            promocion.PorcentajeDecuento = porcentajeDescuento;
        }

        public async void EliminarPromocion(Guid[] idsPromocion)
        {
            List<Promocion> promociones = new List<Promocion>();
            foreach (var idPromocion in idsPromocion)
            {
                var promocion = await _repositorio.ObtenerPromocion(idPromocion);
                if (promocion == null)
                {
                    throw new NullReferenceException();
                }
                promociones.Add(promocion);
            }
            promociones.ForEach(y => _repositorio.ActualizarPromocion(y));
        }

        /// <summary>
        /// Determina si cierta promocion cumple con las condiciones de negocio
        /// para ser modificada o no.
        /// </summary>
        /// <param name="promo"></param>
        /// <returns></returns>
        private async Task<bool> ValidacionesNegocio(Promocion promo)
        {
            foreach (var categoriaProducto in promo.ProductoCategoriaIds)
            {
                if (!await ValidarCategoriaServicio(categoriaProducto))
                {
                    return false;
                }
            }
            foreach (var medioPago in promo.MedioPagoId)
            {
                if (!await ValidarMedioPagoServicio(medioPago))
                {
                    return false;
                }

                if(!await validarMedioPagoFecha(medioPago, promo.FechaInicio, promo.FechaFin))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Valida si la categoria del producto aplica
        /// </summary>
        /// <param name="categoriaProducto"></param>
        /// <returns></returns>
        private async Task<bool> ValidarCategoriaServicio(int categoriaProducto)
        {
            try
            {
                var client = new RestClient();
                client.EndPoint = _rutaServicioMedio;
                client.Method = HttpVerb.GET;
                var json = await client.MakeRequest(categoriaProducto.ToString());
                return true;
            } catch (ApplicationException) {
                return false;
            }
            
        }

        /// <summary>
        /// Valida si el medio de pago es valido
        /// </summary>
        /// <param name="medioPago"></param>
        /// <returns></returns>
        private async Task<bool> ValidarMedioPagoServicio(int medioPago)
        {
            try
            {
                var client = new RestClient();
                client.EndPoint = _rutaServicioCategoria;
                client.Method = HttpVerb.GET;
                var json = await client.MakeRequest(medioPago.ToString());
                return true;
            }
            catch (ApplicationException)
            {
                return false;
            }
        }

        /// <summary>
        /// Valida el acoplamiento de la fecha de pago.
        /// </summary>
        /// <param name="medioPago"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        private async Task<bool> validarMedioPagoFecha(int medioPago, DateTime fechaInicio, DateTime fechaFin)
        {
            var acoplados = await _repositorio.ContarPromocionesAcopladas(medioPago, fechaInicio, fechaFin);
            return acoplados <= 0;
        }
    }
}
