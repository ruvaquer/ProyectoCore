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
    public class Editar
    {
        public class Ejecuta : IRequest{
            public Guid InstructorId{get; set;}
            public string Nombre{get; set;}
            public string Apellidos{get; set;}
            public string Grado{get; set;}
        }

        public class EjecutaValida : AbstractValidator<Ejecuta>{
            public EjecutaValida(){
                RuleFor(x=>x.Nombre).NotEmpty();
                RuleFor(x=>x.Apellidos).NotEmpty();
                RuleFor(x=>x.Grado).NotEmpty();
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
                  InstructorId = request.InstructorId,
                  Nombre = request.Nombre,
                  Apellidos = request.Apellidos,
                  Grado = request.Grado,
                };
                var resultado = await _instructorRepository.Actualiza(instructorModel);

                if(resultado != 0){
                    return Unit.Value;
                }

                throw new Exception("no se pudo actualizar el instructor "+resultado);
            }
        }
    }

}