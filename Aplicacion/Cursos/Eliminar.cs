using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.HandLerError;
using MediatR;
using Persistencia;

namespace Aplicacion.Cursos
{
    public class Eliminar
    {
        public class Ejecuta : IRequest{

            public Guid Id {get; set;}
        
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
                //1º recuperamos los instructores relacionados con el curso para eliminar la relación antes de eliminar los cursos
                var instructoresBD = _context.CursoInstructor.Where(x => x.CursoId == request.Id);
                foreach(var instrunctor in instructoresBD){
                    _context.CursoInstructor.Remove(instrunctor);
                }
                //2º Obtener la lista de comentarios para eliminarlos
                var comentariosBD = _context.Comentario.Where(x => x.CursoId == request.Id);
                foreach(var cmt in comentariosBD){
                    _context.Comentario.Remove(cmt);
                }
                //3º Eliminar el Precio
                var precioBD = _context.Precio.Where(x => x.CursoId == request.Id).FirstOrDefault();
                if(precioBD != null){
                    _context.Precio.Remove(precioBD);
                }        
                //4º Elimino el curso que quiero sin problema ya que no tiene referencias dependientes 
                var curso = await _context.Curso.FindAsync(request.Id);
                if(curso == null){
                    //Reemplazo la excepción general por la que nosotros hemos construido
                    //throw new Exception("No se puede eliminar el curso");
                    //Tipo de error (como no encuentra el curso) es NotFound y 
                    //Nos pide el objeto le mandamos un mensaje en la propiedad curso podemos poner al nombre xxx en vez de curso por ejemplo
                    throw new HandLerException(HttpStatusCode.NotFound, new {mensaje = "No encontro el curso. "});
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