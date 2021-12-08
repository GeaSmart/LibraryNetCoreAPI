using LibraryNetCoreAPI.DTO;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryNetCoreAPI.Controllers
{
    [ApiController]
    [Route("api")]
    public class RootController:ControllerBase
    {
        [HttpGet(Name = "obtenerRoot")]
        public ActionResult<IEnumerable<HateoasDTO>> Get()
        {
            var datosHateoas = new List<HateoasDTO>();
            datosHateoas.Add(new HateoasDTO(enlace:Url.Link("obtenerRoot", new { }), descripcion: "self", metodo: "GET"));

            datosHateoas.Add(new HateoasDTO(enlace: Url.Link("obtenerAutores", new { }), descripcion: "autores", metodo: "GET"));
            datosHateoas.Add(new HateoasDTO(enlace: Url.Link("crearAutor", new { }), descripcion: "autor-crear", metodo: "POST"));

            datosHateoas.Add(new HateoasDTO(enlace: Url.Link("crearLibro", new { }), descripcion: "libro-crear", metodo: "POST"));            


            return datosHateoas;
        }
    }
}
