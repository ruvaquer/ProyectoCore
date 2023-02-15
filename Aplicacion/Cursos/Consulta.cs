using System.Collections.Generic;
using MediatR;
using Dominio;
using System.Threading.Tasks;
using System.Threading;
using Persistencia;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace Aplicacion.Cursos
{
    public class Consulta
    {
        //Esta clase representa la lista de Cursos
        public class ListaCursos : IRequest<List<CursoDto>>{}


        //Esta otra clase representa la lógica de la operación en si. Handler == Manejador puedes poner también majejador en castellano si te parece más comodo
        public class Handler : IRequestHandler<ListaCursos, List<CursoDto>>
        {
            //Propiedad que representa el CursosOnlineContext
            private readonly CursosOnlineContext _context;
            private readonly IMapper _mapper;


            public Handler(CursosOnlineContext context, IMapper mapper){
                _context = context; //<-- Inyection
                _mapper = mapper;
            }

            //Aquí es donde importo o consumo al objeto dle entity Frameworck llamando a la BBDD y devolever la lista de cursos (Toda la lógica de negocio)
            public async Task<List<CursoDto>> Handle(ListaCursos request, CancellationToken cancellationToken)
            {
                //Así solo devuelvo el curso
                //var cursos = await _context.Curso.ToListAsync();
                //Así condigo que me devuelva los datos de los cursos y además los instructores relacionados con el curso
                var cursos = await _context.Curso
                .Include(x => x.InstructoresLink)
                .ThenInclude(x => x.Instructor).ToListAsync();

                //Mappeo de la clase curso (EntityFramework a Dto)
                var cursoDto = _mapper.Map<List<Curso>,List<CursoDto>>(cursos);
                
                return cursoDto;

            }
        }
    }
}