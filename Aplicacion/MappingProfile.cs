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
            //Debemos agregar un mapeo manual para que me retorne la lista de instructores, InstructoresLink representa a la tabla CursoInstructor 
            //y es este quien con un select trae a instructor, además traigo los comentarios y el precio
            CreateMap<Curso,CursoDto>()
            .ForMember(x => x.Instructores, y => y.MapFrom(z => z.InstructoresLink.Select(a => a.Instructor).ToList()))//Lista de instructores del curso
            .ForMember(x => x.comentarios, y => y.MapFrom(z => z.ComentarioLista))//Lista de comentarios del curso
            .ForMember(x => x.precio, y => y.MapFrom(z => z.PrecioPromocion));//Precio del curso
            
            CreateMap<CursoInstructor,CursoInstructorDto>();
            CreateMap<Instructor,InstructorDto>();
            CreateMap<Comentario,ComentarioDto>();
            CreateMap<Precio,PrecioDto>();
        }
    }
}