using LibraryNetCoreAPI.Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryNetCoreAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AutoresController:ControllerBase
    {
        private readonly ApplicationDBContext context;

        public AutoresController(ApplicationDBContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<List<Autor>> Get()
        {
            return await context.Autores.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Autor autor)
        {
            var existe = await context.Autores.AnyAsync(x => x.Nombre == autor.Nombre);
            if (existe)
                return BadRequest($"Ya existe un autor con el nombre {autor.Nombre}");

            context.Add(autor);
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}
