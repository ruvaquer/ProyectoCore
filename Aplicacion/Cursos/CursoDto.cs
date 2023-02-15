using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aplicacion.Cursos
{
    public class CursoDto
    {
        public Guid CursoId {get; set;}
        public string Titulo {get; set;}
        public string Descripcion {get; set;}
        public DateTime? FechaPublicacion {get; set;}
        public byte[] FotoPortada {get; set;}

        //CADA CURSO TIENE NA LISTA DE INSTRUCTORES
        public ICollection<InstructorDto> Instructores {get; set;}
    }
}