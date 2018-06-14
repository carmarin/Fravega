using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Promociones.Domain.Core;
using Promociones.Domain.Entities;
using Promociones.Infrastructure;

namespace Presentation.Api.Controllers
{

    [Produces("application/json")]
    [Route("api/Promociones")]
    public class PromocionesController : Controller
    {
        private readonly PromocionBl promocionBl;

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="promocionBl"></param>
        public PromocionesController(PromocionBl promocionBl)
        {
            this.promocionBl = promocionBl;
        }

        #region Métodos Api

        /// <summary>
        /// Obtiene el listado de promociones
        /// </summary>
        /// <returns></returns>
        //GET Api/Promociones
        [HttpGet]
        [Route("")]
        public Task<String> ListaPromociones()
        {
            return getListaPromociones();
        }

        //GET Api/Promociones/2F714D7E-1CD6-4E73-98A7-98F875D558F6
        [HttpGet]
        [Route("{idPromocion:Guid}")]
        public Task<String> Get(Guid idPromocion)
        {
            return getPromocion(idPromocion);
        }

        /// <summary>
        /// Obtiene todas las promociones en un rango de fechas determinado.
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        //GET Api/Promociones/3
        [HttpGet]
        [Route("date/{fecha:datetime}")]
        public Task<String> ListaPromocionesPorFecha(DateTime fecha)
        {
            return getListaPromocionesPorFecha(fecha);
        }


        /// <summary>
        /// Obtiene las promociones que se encuentran vigentes.
        /// </summary>
        /// <returns></returns>
        // GET Api/Promociones/Vigentes/
        [HttpGet]
        [Route("Vigentes")]
        public Task<String> ListaPromocionesVigentes()
        {
            return getListaPromocionesVigentes();
        }

        //GET Api/Promociones/Venta
        [HttpGet]
        [Route("Venta")]
        public Task<String> ListaPromocionesParaVenta(int idMedioPago, int idTipoMedioPago, int entidadFinanciera, int cantidadCuotas, int categoriaProducto)
        {
            return getListaPromocionesParaVenta(idMedioPago, idTipoMedioPago, entidadFinanciera, cantidadCuotas, categoriaProducto);
        }

        //PUT Api/Promociones/1
        [HttpPut]
        public HttpResponseMessage ModificarPromocion(Guid idPromocion, int[] idMedioPago, int[] idTipoMedioPago, int[] entidadFinanciera, int? cantidadCuotas, int[] categoriaProducto, int porcentajeDescuento)
        {
            try
            {
                var response = new HttpResponseMessage();
                response.StatusCode = HttpStatusCode.OK;
                promocionBl.ModificarPromocion(idPromocion, idMedioPago, idTipoMedioPago, entidadFinanciera, cantidadCuotas, categoriaProducto, porcentajeDescuento);
                return response;
            }
            catch (NullReferenceException)
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }
        }

        //Delete Api/Promociones/1
        [HttpDelete("{id}")]
        public HttpResponseMessage EliminarPromocion(Guid[] idPromocion)
        {
            try
            {
                var response = new HttpResponseMessage();
                response.StatusCode = HttpStatusCode.OK;
                promocionBl.EliminarPromocion(idPromocion);
                return response;
            }
            catch (NullReferenceException)
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }
        }

        //Delete Api/Promociones/1/Vigente
        [HttpGet("{id}/Vigente")]
        public HttpResponseMessage Vigente(Guid idPromocion)
        {
            try
            {
                var response = new HttpResponseMessage();
                response.StatusCode = HttpStatusCode.OK;
                var promocion = promocionBl.ObtenerPromocion(idPromocion);
                if (promocion.Result != null && promocion.Result.Activo)
                {
                    response.Content = new StringContent("true");
                }
                else
                {
                    response.Content = new StringContent("false");
                }

                return response;
            }
            catch (NullReferenceException)
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }
        }
        #endregion

        #region HelpersAsync
        private async Task<string> getPromocion(Guid idPromocion)
        {
            var promociones = await promocionBl.ObtenerPromocion(idPromocion);
            return JsonConvert.SerializeObject(promociones);
        }

        


        private async Task<string> getListaPromocionesPorFecha(DateTime fecha)
        {
            var promociones = await promocionBl.ObtenerPromocionesPorFecha(fecha);
            return JsonConvert.SerializeObject(promociones);
        }

        /// <summary>
        /// Método Helper para manejar la asincronía.
        /// </summary>
        /// <returns></returns>
        private async Task<string> getListaPromociones()
        {
            var promociones = await promocionBl.ObtenerTodasPromociones();
            return JsonConvert.SerializeObject(promociones);
        }

        

        private async Task<string> getListaPromocionesVigentes()
        {
            var promociones = await promocionBl.ObtenerPromocionesVigentes();
            return JsonConvert.SerializeObject(promociones);
        }



       

        private async Task<string> getListaPromocionesParaVenta(int idMedioPago, int idTipoMedioPago, int entidadFinanciera, int cantidadCuotas, int categoriaProducto)
        {
            var promociones = await promocionBl.ObtenerPromocionesVigentes(idMedioPago, idTipoMedioPago, entidadFinanciera, cantidadCuotas, categoriaProducto);
            return JsonConvert.SerializeObject(promociones);
        }
        #endregion

    }
}