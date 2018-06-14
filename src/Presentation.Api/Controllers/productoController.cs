using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Presentation.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/producto")]
    public class productoController : Controller
    {
        

        // GET: api/producto/categorias/2
        [HttpGet("/categorias/{id}")]
        public HttpResponseMessage GetCategoria(int id)
        {
            
            var CategoriaProductos =  new [] { new { id = 1, descripcion = "Categoria 1" }, new { id = 2, descripcion = "Categoria 2" }, new { id = 3, descripcion = "Categoria 3" } };
            var elemento = CategoriaProductos.Where(x => x.id == id);
            if (elemento != null && elemento.Count() > 0)
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StringContent(JsonConvert.SerializeObject(elemento.FirstOrDefault()));
            }
            return new HttpResponseMessage(HttpStatusCode.NotFound);
        }
        
    }
}
