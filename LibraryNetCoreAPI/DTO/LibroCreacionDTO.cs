using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryNetCoreAPI.DTO
{
    public class LibroCreacionDTO
    {        
        [StringLength(maximumLength: 250)]
        public string Titulo { get; set; }
    }
}
