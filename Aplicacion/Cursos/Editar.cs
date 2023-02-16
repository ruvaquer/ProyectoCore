using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.HandLerError;
using Dominio;
using FluentValidation;
using MediatR;
using Persistencia;

namespace Aplicacion.Cursos
{
    public class Editar
    {
        public class Ejecuta : IRequest{
            public Guid CursoId {get; set;}
            public string Titulo {get; set;}
            public string Descripcion {get; set;}

            //Le pongo que permita null ya que por defecto los dateTime no permiten null
            public DateTime? FechaPublicacion {get; set;}

            public List<Guid> ListaInstructor {get; set;}
        }

        //Creo una nueva clase que me controle la validación que estara entre la clase Ejecuta y el Handler
        public class EjecutaValidacion : AbstractValidator<Ejecuta>{
            //Dentro del constructor agrego las reglas de validación
            public EjecutaValidacion(){
                RuleFor(x => x.Titulo).NotEmpty();
                RuleFor(x => x.Descripcion).NotEmpty();
                RuleFor(x => x.FechaPublicacion).NotEmpty();
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
                //Actualizar los datos del curso
                var curso = await _context.Curso.FindAsync(request.CursoId);
                if(curso == null){
                    //Si falla algo lanzo una alerta de error, solo va ocurrir si no entra en el if anterior
                    throw new HandLerException(HttpStatusCode.NotFound, new {mensaje = "No encontro el curso. "});
                }
                
                //Actualización el operador ?? Evalua que si esta variable que le mando por ejemplo el Titulo es indefinida o null lo que hace es poner le valor que tenía antes
                curso.Titulo = request.Titulo ?? curso.Titulo;
                curso.Descripcion = request.Descripcion ?? curso.Descripcion;
                curso.FechaPublicacion = request.FechaPublicacion ?? curso.FechaPublicacion;

                //Edición de Instructores VALIDACIÓN
                if(request.ListaInstructor != null){
                    //Si tengo valores actualizo
                    if(request.ListaInstructor.Count > 0){
                        //1º Elimino los instructores actuales en la BBDD
                        var instructoresBD = _context.CursoInstructor.Where(x => x.CursoId == request.CursoId).ToList();
                        foreach(var instructorEliminar in instructoresBD){
                            _context.CursoInstructor.Remove(instructorEliminar);
                        }
                        //**********FIN DEL PROCEDIMIENTO PARA ELIMINAR INSTRUCTORES**********
                        
                        //Ahora procedimiento para agregar instructores que provienen del cliente 
                        foreach(var ids in request.ListaInstructor){
                            var nuevoInstuctor = new CursoInstructor{
                                CursoId = request.CursoId,
                                InstructorId = ids
                            };
                            _context.CursoInstructor.Add(nuevoInstuctor);
                        }
                        //*************FIN DEL PROCEDIMIENTO*****************

                    }
                }

                //Transacción de confirmación en la BBDD
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