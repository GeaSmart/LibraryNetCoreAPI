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
    [Route("api/libros/{libroId:int}/comentarios")]
    public class ComentariosController : ControllerBase
    {
        private readonly ApplicationDBContext context;
        private readonly IMapper mapper;

        public ComentariosController(ApplicationDBContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<ComentarioDTO>>> Get(int libroId)
        {
            var existeLibro = await context.Libros.AnyAsync(x => x.Id == libroId);
            if (!existeLibro)
                return NotFound($"El libro con id {libroId} no existe");

            var comentarios = await context.Comentarios.Where(x => x.LibroId == libroId).ToListAsync();
            return mapper.Map<List<ComentarioDTO>>(comentarios);
        }

        [HttpPost]
        public async Task<ActionResult> Post(int libroId, ComentarioCreacionDTO comentarioCreacionDTO)
        {
            var existeLibro = await context.Libros.AnyAsync(x => x.Id == libroId);
            if (!existeLibro)
                return NotFound($"El libro con id {libroId} no existe");

            var comentario = mapper.Map<Comentario>(comentarioCreacionDTO);
            comentario.LibroId = libroId;
            context.Comentarios.Add(comentario);
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}
