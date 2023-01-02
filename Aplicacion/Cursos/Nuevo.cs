using System;
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

            //Cancelación token se usa cuando un cliente cancela la petición por ejemplo cancelando un peddo por un cliente así se cancela a nivel de back y servidor
            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var curso = new Curso{
                    Titulo = request.Titulo,
                    Descripcion = request.Descripcion,
                    FechaPublicacion = request.FechaPublicacion //Si ponemos que puede ser nulo debemos indicar que permite nulos en la clase Curso para que permita null igual que se lo indicamos en el request
                };

                _context.Curso.Add(curso);
                //Ahora confirmamos la ransacción
                var valor = await _context.SaveChangesAsync();//Devuelve un estado de la transacción, si es 0 significa que hemos tenido algún error si es 1 o superior es que es correcto.
                if(valor > 0){
                    return Unit.Value;
                }

                //Si falla algo lanzo una alerta de error, solo va ocurrir si no entra en el if anterior
                throw new Exception("No se pudo insertar el Curso");

            }
        }
    }
}