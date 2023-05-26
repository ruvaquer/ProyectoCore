using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.HandLerError;
using MediatR;
using Persistencia.DapperConexion.Instructor;

namespace Aplicacion.Instructores
{
    public class ConsultaId
    {
        public class Ejecuta : IRequest<InstructorModel>{
            public Guid Id{get; set;}
            
        }

        public class Handler : IRequestHandler<Ejecuta,InstructorModel>
        {
            private readonly IInstructor _instructorRepository;

            public Handler(IInstructor instructorRepository){
                _instructorRepository = instructorRepository;
            }

            public async Task<InstructorModel> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var instructor = await _instructorRepository.ObtenerPorId(request.Id);

                if(instructor == null){
                    throw new HandLerException(HttpStatusCode.NotFound, new {mensaje = "No se encontro el instructor"});
                }
                return instructor;
            }
        }
    }

}