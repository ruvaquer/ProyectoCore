using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.HandLerError;
using Dominio;
using MediatR;
using Persistencia;

namespace Aplicacion.Cursos
{
    public class ConsultaId
    {
        public class CursoUnico : IRequest<Curso>{
            public int Id {get; set;}
        }

        public class Handler : IRequestHandler<CursoUnico, Curso>
        {
            private readonly CursosOnlineContext _context;
            public Handler(CursosOnlineContext context)
            {
                _context = context;
            }

            public async Task<Curso> Handle(CursoUnico request, CancellationToken cancellationToken)
            {
                var curso = await _context.Curso.FindAsync(request.Id);
                
                if(curso == null){
                    //Si falla algo lanzo una alerta de error, solo va ocurrir si no entra en el if anterior
                    throw new HandLerExcepcion(HttpStatusCode.NotFound, new {mensaje = "No encontro el curso. "});
                }
                
                return curso;
            }
        }
    }
}