using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Promociones.Domain.Core;
using Promociones.Domain.Entities;

namespace Presentation.Api.Controllers
{
    /// <summary>
    /// Controladora creada para manejar las acciones de la creación
    /// inicial de datos para tener una semilla para las operaciones del API
    /// </summary>
    [Produces("application/json")]
    [Route("api/Init")]
    public class InitController : Controller
    {
        private readonly PromocionBl promocionBl;

        public InitController(PromocionBl promocionBl)
        {
            this.promocionBl = promocionBl;
        }

        /// <summary>
        /// Método encargado de crear los datos iniciales del API
        /// </summary>
        /// <param name="setting">init: En caso de querer crear los datos iniciales del API</param>
        /// <returns></returns>
        // Init/inicializar/init
        [HttpGet("inicializar/{setting}")]
        public Task<string> EjecutarInit(string setting)
        {
            return ejecutarInit(setting);

        }

        /// <summary>
        /// Método asincronico encargado de manejar las operaciones de 
        /// creación de datos iniciales de la aplicación
        /// </summary>
        /// <param name="setting">Init para crear los datos</param>
        /// <returns>Un string indicando el resultado de la operacion hecho si todo salio bien
        /// Desconocido si se envio un comando desconocido a la acción</returns>
        private async Task<string> ejecutarInit(string setting)
        {
            if (setting == "init")
            {
                await promocionBl.EliminarPromociones();
                await promocionBl.AgregarPromocion(new Promocion()
                {
                    Activo = true,
                    TipoMedioPagoId = null,
                    EntidadFinancieraId = null,
                    MedioPagoId = new int[] { 1, 2 },
                    MaxCantidadDeCuotas = 12,
                    PorcentajeDecuento = 15,
                    FechaInicio = Convert.ToDateTime("01/06/2018"),
                    FechaFin = Convert.ToDateTime("01/06/2019"),
                    FechaCreacion = DateTime.Now,
                    ProductoCategoriaIds = new int[] { 1, 2 }
                });

                await promocionBl.AgregarPromocion(new Promocion()
                {
                    Activo = true,
                    TipoMedioPagoId = null,
                    EntidadFinancieraId = new int[] { 1 },
                    MedioPagoId = null,
                    MaxCantidadDeCuotas = 12,
                    PorcentajeDecuento = 10,
                    FechaInicio = Convert.ToDateTime("01/06/2018"),
                    FechaFin = Convert.ToDateTime("01/06/2019"),
                    FechaCreacion = DateTime.Now,
                    ProductoCategoriaIds = new int[] { 1, 2 }
                });

                await promocionBl.AgregarPromocion(new Promocion()
                {
                    Activo = true,
                    TipoMedioPagoId = new int[] { 1, 2 },
                    EntidadFinancieraId = null,
                    MedioPagoId = null,
                    MaxCantidadDeCuotas = 12,
                    PorcentajeDecuento = 20,
                    FechaInicio = Convert.ToDateTime("01/06/2018"),
                    FechaFin = Convert.ToDateTime("01/06/2019"),
                    FechaCreacion = DateTime.Now,
                    ProductoCategoriaIds = new int[] { 1, 2 }
                });

                await promocionBl.AgregarPromocion(new Promocion()
                {
                    Activo = true,
                    TipoMedioPagoId = null,
                    EntidadFinancieraId = null,
                    MedioPagoId = new int[] { 10 },
                    MaxCantidadDeCuotas = null,
                    PorcentajeDecuento = 25,
                    FechaInicio = Convert.ToDateTime("01/06/2018"),
                    FechaFin = Convert.ToDateTime("01/06/2019"),
                    FechaCreacion = DateTime.Now,
                    ProductoCategoriaIds = new int[] { 1, 2 }
                });

                await promocionBl.AgregarPromocion(new Promocion()
                {
                    Activo = true,
                    TipoMedioPagoId = new int[] { 1 },
                    EntidadFinancieraId = new int[] { 1 },
                    MedioPagoId = null,
                    MaxCantidadDeCuotas = 12,
                    PorcentajeDecuento = 5,
                    FechaInicio = Convert.ToDateTime("01/06/2018"),
                    FechaFin = Convert.ToDateTime("01/06/2019"),
                    FechaCreacion = DateTime.Now,
                    ProductoCategoriaIds = new int[] { 1, 2 }
                });
                return "Hecho";
            }

            return "Desconocido";

        }

    }
}
