using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dominio;
using FluentValidation;
using MediatR;
using Persistencia;

namespace Aplicacion.Comentarios
{
    public class Nuevo
    {
        /*
        Nota a parte: que me traiga la cantidad de cursos en las cuales esta asignado cada instructor
        var cursosInstructores = await _context.CursoInstructor.Where(x => x.InstructorId == request.InstructorId).ToListAsync();
        var cantidadCursos = cursosInstructores.Count();
        */
        public class Ejecuta : IRequest
        {
            public string Alumno { get; set; }
            public int Puntuacion { get; set; }
            public string Comentario { get; set; }
            public Guid CursoId { get; set; }
        }

        public class EjecutaValidacion : AbstractValidator<Ejecuta>
        {
            public EjecutaValidacion()
            {
                RuleFor(x => x.Alumno).NotEmpty();
                RuleFor(x => x.Puntuacion).NotEmpty();
                RuleFor(x => x.Comentario).NotEmpty();
                RuleFor(x => x.CursoId).NotEmpty();
            }
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
                var comentario = new Comentario()
                {
                    ComentarioId = Guid.NewGuid(),
                    Puntuacion = request.Puntuacion,
                    Alumno = request.Alumno,
                    ComentarioTexto = request.Comentario,
                    CursoId = request.CursoId
                };

                _context.Comentario.Add(comentario);

                var resultado = await _context.SaveChangesAsync();

                if (resultado > 0)
                {
                    return Unit.Value;
                }

                throw new Exception("No se pudo insertar el comentario");
            }
        }
    }
}