using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.HandLerError;
using MediatR;
using Persistencia;

namespace Aplicacion.Comentarios
{
    public class Eliminar
    {
        /*
        Nota a parte: que me traiga la cantidad de cursos en las cuales esta asignado cada instructor
        var cursosInstructores = await _context.CursoInstructor.Where(x => x.InstructorId == request.InstructorId).ToListAsync();
        var cantidadCursos = cursosInstructores.Count();
        */
        public class Ejecuta : IRequest
        {
            public Guid Id {get; set;}
            
        }

        public class Handler : IRequestHandler<Ejecuta>
        {

            private readonly CursosOnlineContext _context;
            public Handler(CursosOnlineContext context){
                _context = context;
            }

            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var comenrtario =  await _context.Comentario.FindAsync(request.Id);

                if(comenrtario == null){
                    throw new HandLerException(HttpStatusCode.NotFound, new {mensaje = "No se encontro el comentario."});
                }

                _context.Remove(comenrtario);

                var resultado = await _context.SaveChangesAsync();

                if (resultado > 0)
                {
                    return Unit.Value;
                }

                throw new Exception("No se pudo eliminar el comentario");
            }
        }
    }
}