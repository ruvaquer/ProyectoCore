using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Persistencia.DapperConexion.Instructor
{
    public class InstructorModel
    {
        public Guid InstructorId {get; set;}
        public String Nombre {get; set;}
        public String Apellidos {get; set;}
        public String Grado {get; set;}
    }
}