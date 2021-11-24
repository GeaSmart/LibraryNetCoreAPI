using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryNetCoreAPI.Entidades
{
    public class Comentario
    {
        public int Id { get; set; }

        [Required]
        public string Contenido { get; set; }
        public int LibroId { get; set; }

        //Propiedad de navegación
        public Libro Libro { get; set; }
    }
}
