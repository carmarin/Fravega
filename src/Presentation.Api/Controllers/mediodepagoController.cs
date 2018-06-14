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
    [Route("api/mediodepago")]
    public class mediodepagoController : Controller
    {

        // GET: api/mediodepago/5
        [HttpGet("{id}", Name = "Get")]
        public HttpResponseMessage Get(int id)
        {
            var medioPago = new[] { new { id = 10, descripcion = "Efectivo" }, new { id = 1, descripcion = "Visa" }, new { id = 2, descripcion = "Master Card" } };
            var elemento = medioPago.Where(x => x.id == id);
            if (elemento != null && elemento.Count() > 0)
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StringContent(JsonConvert.SerializeObject(elemento.FirstOrDefault()));
            }
            return new HttpResponseMessage(HttpStatusCode.NotFound);
        }
    }
}
