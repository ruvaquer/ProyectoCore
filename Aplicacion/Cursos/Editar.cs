using System;
using System.Threading;
using System.Threading.Tasks;
using Dominio;
using MediatR;
using Persistencia;

namespace Aplicacion.Cursos
{
    public class Editar
    {
        public class Ejecuta : IRequest{
            public int CursoId {get; set;}
            public string Titulo {get; set;}
            public string Descripcion {get; set;}

            //Le pongo que permita null ya que por defecto los dateTime no permiten null
            public DateTime? FechaPublicacion {get; set;}
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
                var curso = await _context.Curso.FindAsync(request.CursoId);
                if(curso == null){
                    //Si falla algo lanzo una alerta de error, solo va ocurrir si no entra en el if anterior
                    throw new Exception("El curso buscado no existe.");
                }
                
                //Actualización el operador ?? Evalua que si esta variable que le mando por ejemplo el Titulo es indefinida o null lo que hace es poner le valor que tenía antes
                curso.Titulo = request.Titulo ?? curso.Titulo;
                curso.Descripcion = request.Descripcion ?? curso.Descripcion;
                curso.FechaPublicacion = request.FechaPublicacion ?? curso.FechaPublicacion;

                //Transacción
                var resultado = await _context.SaveChangesAsync();//Devuelve un estado de la transacción, si es 0 significa que hemos tenido algún error si es 1 o superior es que es correcto.
                
                if(resultado > 0){
                    return Unit.Value;
                }

                //Si falla algo lanzo una alerta de error, solo va ocurrir si no entra en el if anterior
                throw new Exception("No se pudo actualizar el Curso");
            }
        }
    }
}