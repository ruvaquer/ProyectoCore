using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Persistencia.DapperConexion.Instructor;

namespace Aplicacion.Instructores
{
    public class Nuevo
    {
        public class Ejecuta : IRequest{
            //Parametros que llegan desde cliente el Id no hace falta ya que lo creo internamente
            public string Nombre {get; set;}
            public string Apellidos {get; set;}
            public string Grado {get; set;}
        }

        //Clase que valida los datos
        public class EjecutaValida : AbstractValidator<Ejecuta>{
            public EjecutaValida(){
                RuleFor(x => x.Nombre).NotEmpty();
                RuleFor(x => x.Apellidos).NotEmpty();
                RuleFor(x => x.Grado).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Ejecuta>
        {
            private readonly IInstructor _instructorRepository;

            public Handler(IInstructor instructorRepository){
            _instructorRepository = instructorRepository;
            }

            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var instructorModel = new InstructorModel{
                  Nombre = request.Nombre,
                  Apellidos = request.Apellidos,
                  Grado = request.Grado,
                };
                var resultado = await _instructorRepository.Nuevo(instructorModel);

                if(resultado > 0){
                    return Unit.Value;
                }

                throw new Exception("no se pudo insertar el instructor "+resultado);

            }
        }

    }
}