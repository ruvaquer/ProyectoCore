using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aplicacion.Cursos;
using AutoMapper;
using Dominio;

namespace Aplicacion
{
    public class MappingProfile :Profile
    {
        public MappingProfile(){
            CreateMap<Curso,CursoDto>();
             CreateMap<CursoInstructor,CursoInstructorDto>();
            CreateMap<Instructor,InstructorDto>();
        }
    }
}