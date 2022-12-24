using System.Collections.Generic;
using MediatR;
using Dominio;
using System.Threading.Tasks;
using System.Threading;
using Persistencia;
using Microsoft.EntityFrameworkCore;

namespace Aplicacion.Cursos
{
    public class Consulta
    {
        //Esta clase representa la lista de Cursos
        public class ListaCursos : IRequest<List<Curso>>{}


        //Esta otra clase representa la lógica de la operación en si. Handler == Manejador puedes poner también majejador en castellano si te parece más comodo
        public class Handler : IRequestHandler<ListaCursos, List<Curso>>
        {
            //Propiedad que representa el CursosOnlineContext
            private readonly CursosOnlineContext _context;

            public Handler(CursosOnlineContext context){
                _context = context; //<-- Inyection
            }

            //Aquí es donde importo o consumo al objeto dle entity Frameworck llamando a la BBDD y devolever la lista de cursos (Toda la lógica de negocio)
            public async Task<List<Curso>> Handle(ListaCursos request, CancellationToken cancellationToken)
            {
                var cursos = await _context.Curso.ToListAsync();
                return cursos;

            }
        }
    }
}