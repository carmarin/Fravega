using Microsoft.Extensions.Configuration;
using Moq;
using Presentation.Api.Controllers;
using Promociones.Domain.Core;
using Promociones.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using Test;
using Xunit;

namespace Promociones.Test
{
    public class InitControllerTest
    {
        [Theory]
        [InlineData("prueba")]
        [InlineData("desconocido")]
        [InlineData("test")]
        public void EjecutarInit_PalabraDiferente_RetornaDesconocido(string palabra)
        {
            var repositorio = new Mock<IPromocionRepository>();
            var icfg = new Mock<IConfiguration>();
            PromocionBl promocionBl = new PromocionBl(repositorio.Object, icfg.Object);
            var controller = new InitController(promocionBl);
            var resp = controller.EjecutarInit(palabra);
            Assert.Equal("Desconocido", resp.Result);
        }

        [Fact]
        public void EjecutarInit_PalabraInit_CreaElementos()
        {
            var repositorio = MockHelper.GenerarMockRepositorio();
            var icfg = new Mock<IConfiguration>();
            PromocionBl promocionBl = new PromocionBl(repositorio.Object, icfg.Object);
            var controller = new InitController(promocionBl);
            var resp = controller.EjecutarInit("init");
            Assert.True(repositorio.Object.ObtenerTodasPromociones().Result.GetEnumerator().MoveNext());
        }
    }
}
