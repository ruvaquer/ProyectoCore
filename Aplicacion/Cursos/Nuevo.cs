using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using Dominio;
using FluentValidation;
using MediatR;
using Persistencia;

namespace Aplicacion.Cursos
{
    public class Nuevo
    {
        //Mapeamos los datos que llegan al controler
        public class Ejecuta : IRequest{
            public string Titulo {get; set;}
            public string Descripcion {get; set;}
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

        //Handler o manejador que se encarga de hacer la transaccion en la BBDD
        public class Handler : IRequestHandler<Ejecuta>
        {
            private readonly CursosOnlineContext _context;

            public Handler(CursosOnlineContext context)
            {
                _context = context;
            }

            //Cancelación token se usa cuando un cliente cancela la petición por ejemplo cancelando un peddo por un cliente así se cancela a nivel de back y servidor
            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                Guid _cursoId = Guid.NewGuid(); //Te crea un valor aleatorio que sera nuestro Id
                var curso = new Curso{
                    CursoId = _cursoId,
                    Titulo = request.Titulo,
                    Descripcion = request.Descripcion,
                    FechaPublicacion = request.FechaPublicacion //Si ponemos que puede ser nulo debemos indicar que permite nulos en la clase Curso para que permita null igual que se lo indicamos en el request
                };

                _context.Curso.Add(curso);

                //Llamamos a la lista de Instructores para insertar la lista de instructores que va tener mi curso
                //Si no hay instructor no pasa nada no entrará en esta lógica y grabara solo el curso
                if(request.ListaInstructor !=null){
                    foreach(var id in request.ListaInstructor){
                        //Por cada id de listInstructor tendre que crear un nuevo record que se almacene en la tabla CursoInstructor
                       var cursoInstructor = new CursoInstructor{
                            CursoId = curso.CursoId,
                            InstructorId = id
                       };
                       _context.CursoInstructor.Add(cursoInstructor);
                    }
                }

                //Ahora confirmamos la ransacción Aqui estoy ejecutando la transaccion tanto de Curso como de CursoInstructores
                var valor = await _context.SaveChangesAsync();//Devuelve un estado de la transacción, si es 0 significa que hemos tenido algún error si es 1 o superior es que es correcto.
                //Valor tiene el número de operaciones que se realizan sobre la data en la BBDD
                if(valor > 0){
                    return Unit.Value;
                }

                //Si falla algo lanzo una alerta de error, solo va ocurrir si no entra en el if anterior
                throw new Exception("No se pudo insertar el Curso");

            }
        }
    }
}