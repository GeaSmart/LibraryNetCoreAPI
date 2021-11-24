using AutoMapper;
using LibraryNetCoreAPI.DTO;
using LibraryNetCoreAPI.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryNetCoreAPI.Utils
{
    public class AutoMapperProfiles:Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AutorCreacionDTO, Autor>();
            CreateMap<Autor, AutorDTO>();
            CreateMap<Libro, LibroDTO>()
                .ForMember(x => x.AutoresDTO, options => options.MapFrom(MapAutoresLibroDTO));
            CreateMap<LibroCreacionDTO, Libro>()
                .ForMember(x => x.AutoresLibros, options => options.MapFrom(MapAutoresLibros));
            CreateMap<ComentarioCreacionDTO, Comentario>();
            CreateMap<Comentario, ComentarioDTO>();
        }

        private List<AutorDTO> MapAutoresLibroDTO(Libro libro, LibroDTO libroDTO)
        {
            List<AutorDTO> response = new List<AutorDTO>();
            if (libro.AutoresLibros == null)
                return response;

            foreach(var item in libro.AutoresLibros)
            {
                response.Add(new AutorDTO { Id = item.Autor.Id, Nombre = item.Autor.Nombre });
            }
            return response;
        }

        private List<AutorLibro> MapAutoresLibros(LibroCreacionDTO libroCreacionDTO, Libro libro)
        {
            List<AutorLibro> response = new List<AutorLibro>();

            if (libroCreacionDTO.AutoresIds == null)
                return response;
            
            foreach(var item in libroCreacionDTO.AutoresIds)
            {
                response.Add(new AutorLibro { AutorId = item });
            }

            return response;
        }
    }
}
