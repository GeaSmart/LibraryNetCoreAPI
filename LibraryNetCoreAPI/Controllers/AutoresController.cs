﻿using AutoMapper;
using LibraryNetCoreAPI.DTO;
using LibraryNetCoreAPI.Entidades;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryNetCoreAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "isAdmin")]
    public class AutoresController:ControllerBase
    {
        private readonly ApplicationDBContext context;
        private readonly IMapper mapper;
        private readonly IConfiguration configuration;

        public AutoresController(ApplicationDBContext context, IMapper mapper, IConfiguration configuration)
        {
            this.context = context;
            this.mapper = mapper;
            this.configuration = configuration;
        }

        [HttpGet("AppConfiguration")]
        public ActionResult<List<string>> GetConfiguration()
        {
            var lista = new List<string>();
            lista.Add(configuration["app-author"]);
            lista.Add(configuration["connectionStrings:defaultConnection"]);
            lista.Add(configuration.GetConnectionString("defaultConnection"));
            lista.Add(configuration["user-secret-1"]);
            lista.Add(configuration["test"]);

            return lista;
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<List<AutorDTO>> Get()
        {
            var autores = await context.Autores.ToListAsync();
            return mapper.Map<List<AutorDTO>>(autores);
        }

        [HttpGet("{id:int}", Name = "ObtenerAutor")]
        public async Task<ActionResult<AutorConLibrosDTO>> Get(int id)
        {
            var autor = await context.Autores
                .Include(x=>x.AutoresLibros)
                .ThenInclude(x=>x.Libro)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (autor == null)
                return NotFound($"El autor con id {id} no existe.");

            return mapper.Map<AutorConLibrosDTO>(autor);
        }

        [HttpGet("{nombre}")]
        public async Task<ActionResult<List<AutorDTO>>> Get([FromRoute] string nombre)
        {
            var autores = await context.Autores.Where(x => x.Nombre.Contains(nombre)).ToListAsync();
            return mapper.Map<List<AutorDTO>>(autores);
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

            var autorDTO = mapper.Map<AutorDTO>(autor);
            return CreatedAtRoute("ObtenerAutor", new { id = autor.Id }, autorDTO);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, AutorCreacionDTO autorCreacionDTO)
        {
            var existeAutor = await context.Autores.AnyAsync(x=>x.Id == id);
            if (!existeAutor)
                return NotFound("No existe el autor");

            var autor = mapper.Map<Autor>(autorCreacionDTO);
            autor.Id = id;

            context.Update(autor);
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existeAutor = await context.Autores.AnyAsync(x => x.Id == id);
            if (!existeAutor)
                return NotFound("El autor no existe");

            context.Autores.Remove(new Autor { Id = id });
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
