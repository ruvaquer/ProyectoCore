using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Persistencia;

namespace Aplicacion.Cursos
{
    public class Eliminar
    {
        public class Ejecuta : IRequest{

            public int Id {get; set;}
        
        }

        public class Handler : IRequestHandler<Ejecuta>
        {
            private readonly CursosOnlineContext _context;

            public Handler(CursosOnlineContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var curso = await _context.Curso.FindAsync(request.Id);
                if(curso == null){
                    throw new Exception("No se puede eeliminar el curso");
                }

                _context.Remove(curso);

                //Commit
                var resultado = await _context.SaveChangesAsync();
                if(resultado > 0){
                    return Unit.Value;
                }

                //Si falla algo lanzo una alerta de error, solo va ocurrir si no entra en el if anterior
                throw new Exception("No se pudo actualizar el Curso");

            }
        }
    }
}