using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryNetCoreAPI.Entidades
{
    public class Libro
    {
        public int Id { get; set; }
        [StringLength(maximumLength: 250)]
        public string Titulo { get; set; }
    }
}
