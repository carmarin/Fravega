using Moq;
using Promociones.Domain.Entities;
using Promociones.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public class MockHelper
    {
        /// <summary>
        /// Crea un mock para las operaciones relacionadas con el repositorio
        /// </summary>
        /// <returns></returns>
        public static Mock<IPromocionRepository> GenerarMockRepositorio()
        {
            List<Promocion> arreglo = new List<Promocion>();  
            var repositorio = new Mock<IPromocionRepository>();
            // Mock de agregar promocion
            repositorio.Setup(x => x.AgregarPromocion(It.IsAny<Promocion>())).Returns(
                async (Promocion target) =>
                {
                    await Task.Run(() => arreglo.Add(target));
                });
            // Mock de consultar todos
            repositorio.Setup(x => x.ObtenerTodasPromociones()).Returns(
                async () =>
                {
                    return await Task.Run(() => arreglo);
                });
            return repositorio;

        }
    }
}
