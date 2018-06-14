using Microsoft.Extensions.Configuration;
using Moq;
using Presentation.Api.Controllers;
using Promociones.Domain.Core;
using Promociones.Infrastructure;
using System;
using System.Net;
using Test;
using Xunit;

namespace Promociones.Test
{
    public class PromocionesControllerTest
    {

        [Fact]
        public void Get_ListaPromociones_RetornaVacio()
        {
            var repositorio = MockHelper.GenerarMockRepositorio();
            var icfg = new Mock<IConfiguration>();
            PromocionBl promocionBl = new PromocionBl(repositorio.Object, icfg.Object);
            var controller = new PromocionesController(promocionBl);
            var resp = controller.ListaPromociones();
            Assert.True(resp.Result.Length <= 2);
        }

        [Fact]
        public void Get_Vigente_RetornaVacio()
        {
            var repositorio = new Mock<IPromocionRepository>();
            var icfg = new Mock<IConfiguration>();
            PromocionBl promocionBl = new PromocionBl(repositorio.Object, icfg.Object);
            var controller = new PromocionesController(promocionBl);
            var resp = controller.Vigente(new Guid("9083-9847-9083-9023")) ;
            Assert.True(resp.StatusCode == HttpStatusCode.OK);
        }
    }
}
