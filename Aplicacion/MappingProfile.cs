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
            //Debemos agregar un mapeo manual para que me retorne la lista de instructores, InstructoresLink representa a lka tabla CursoInstructor y es este quien con un select trae a instructor
            CreateMap<Curso,CursoDto>()
            .ForMember(x => x.Instructores, y => y.MapFrom(z => z.InstructoresLink.Select(a => a.Instructor).ToList()));
            
            CreateMap<CursoInstructor,CursoInstructorDto>();
            CreateMap<Instructor,InstructorDto>();
        }
    }
}