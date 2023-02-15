using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.HandLerError;
using Dominio;
using MediatR;
using Persistencia;
using System;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Aplicacion.Cursos
{
    public class ConsultaId
    {
        public class CursoUnico : IRequest<CursoDto>{
            public Guid Id {get; set;}
        }

        public class Handler : IRequestHandler<CursoUnico, CursoDto>
        {
            private readonly CursosOnlineContext _context;
            private readonly IMapper _mapper;
            public Handler(CursosOnlineContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<CursoDto> Handle(CursoUnico request, CancellationToken cancellationToken)
            {
                var curso = await _context.Curso
                .Include(x => x.InstructoresLink)//Instructores link es la lista CursoInstructor y a traves de este recupero los Instrucntores
                .ThenInclude(y => y.Instructor)
                .FirstOrDefaultAsync(a => a.CursoId == request.Id);
                
                if(curso == null){
                    //Si falla algo lanzo una alerta de error, solo va ocurrir si no entra en el if anterior
                    throw new HandLerException(HttpStatusCode.NotFound, new {mensaje = "No encontro el curso. "});
                }
                var cursoDto = _mapper.Map<Curso,CursoDto>(curso);
                return cursoDto;
            }
        }
    }
}