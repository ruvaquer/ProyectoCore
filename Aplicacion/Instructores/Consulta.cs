using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Persistencia.DapperConexion.Instructor;

namespace Aplicacion.Instructores
{
    public class Consulta
    {
        public class Lista : IRequest<List<InstructorModel>>{}

        public class Handler : IRequestHandler<Lista, List<InstructorModel>>
        {
            private readonly IInstructor _instructorRepository;

            public Handler(IInstructor instructorRepository){
                _instructorRepository = instructorRepository;
            }

            public async Task<List<InstructorModel>> Handle(Lista request, CancellationToken cancellationToken)
            {
                //Aqui trabajo la logica que retorna el procedure esta en la sección Persistencia InstructorRepository
                var resultado = await _instructorRepository.ObtenerLista();
                return resultado.ToList();
            }
        }
    }
}