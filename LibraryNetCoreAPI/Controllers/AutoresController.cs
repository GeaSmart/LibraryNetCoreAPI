using AutoMapper;
using LibraryNetCoreAPI.DTO;
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
        private readonly IMapper mapper;

        public AutoresController(ApplicationDBContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<List<Autor>> Get()
        {
            return await context.Autores.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] AutorCreacionDTO autorCreacionDTO)
        {
            var existe = await context.Autores.AnyAsync(x => x.Nombre == autorCreacionDTO.Nombre);
            if (existe)
                return BadRequest($"Ya existe un autor con el nombre {autorCreacionDTO.Nombre}");

            //context.Add(autor); antes le pasábamos una instancia de Autor directamente, ahora mapeamos autorCreacionDTO a Autor y le enviamos
            var autor = mapper.Map<Autor>(autorCreacionDTO);
            context.Add(autor);

            await context.SaveChangesAsync();
            return Ok();
        }
    }
}
