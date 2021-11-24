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
    public class LibrosController : ControllerBase
    {
        private readonly ApplicationDBContext context;
        private readonly IMapper mapper;

        public LibrosController(ApplicationDBContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<LibroDTO>> Get(int id)
        {
            var libro = await context.Libros
                .Include(x => x.AutoresLibros)                
                .ThenInclude(x => x.Autor)                
                .FirstOrDefaultAsync(x => x.Id == id);

            libro.AutoresLibros = libro.AutoresLibros.OrderBy(x=>x.Orden).ToList(); //ordenando la lista de autores por el campo orden
            return mapper.Map<LibroDTO>(libro);
        }

        [HttpPost]
        public async Task<ActionResult>Post ([FromBody] LibroCreacionDTO libroCreacionDTO)
        {
            if (libroCreacionDTO.AutoresIds == null)
                return BadRequest("No se puede crear un libro sin autor");

            var autoresIds = await context.Autores.Where(x => libroCreacionDTO.AutoresIds.Contains(x.Id))
                .Select(x=>x.Id).ToListAsync();

            if (autoresIds.Count != libroCreacionDTO.AutoresIds.Count)
                return BadRequest("Se ingreso al menos un autor no registrado");

            var libro = mapper.Map<Libro>(libroCreacionDTO);

            for(int i = 0; i < libro.AutoresLibros.Count; i++)
            {
                libro.AutoresLibros[i].Orden = i;
            }

            context.Libros.Add(libro);
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}
