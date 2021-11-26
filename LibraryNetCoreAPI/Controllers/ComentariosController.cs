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

        [HttpGet("{id:int}", Name = "ObtenerComentario")]
        public async Task<ActionResult<ComentarioDTO>> GetById(int libroId, int id)
        {
            //el where asegura que el comentario corresponda al libro correcto
            var comentario = await context.Comentarios.Where(x => x.LibroId == libroId).FirstOrDefaultAsync(x => x.Id == id);

            if (comentario == null)
                return NotFound("Comentario no existe");

            return mapper.Map<ComentarioDTO>(comentario);
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

            var comentarioDTO = mapper.Map<ComentarioDTO>(comentario);
            //return Ok(); ya no se devuelve un simple Ok
            return CreatedAtRoute("ObtenerComentario", new { id = comentario.Id, libroId = libroId }, comentarioDTO);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, int libroId,  ComentarioCreacionDTO comentarioCreacionDTO)
        {
            var existeLibro = await context.Libros.AnyAsync(x => x.Id == libroId);
            if (!existeLibro)
                return NotFound($"El libro con id {libroId} no existe");

            var existeComentario = await context.Comentarios.AnyAsync(x => x.Id == id);
            if (!existeComentario)
                return NotFound($"El comentario con id {id} no existe");

            var comentario = mapper.Map<Comentario>(comentarioCreacionDTO);
            comentario.Id = id;
            comentario.LibroId = libroId;

            context.Comentarios.Update(comentario);
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id, int libroId)
        {
            var existeComentario = await context.Comentarios.Where(x => x.LibroId == libroId).AnyAsync(x => x.Id == id);
            if (!existeComentario)
                return NotFound("El comentario no existe o no coincide con el libro especificado");

            context.Comentarios.Remove(new Comentario { Id = id });
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
