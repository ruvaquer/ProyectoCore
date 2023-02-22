using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Persistencia.DapperConexion.Instructor
{
    public class InstructorRepository : IInstructor
    {
        private readonly IFactoryConexion _factoryConnection;
        public InstructorRepository(IFactoryConexion factoryConnection){
            _factoryConnection = factoryConnection;
        }

        public Task<int> Actualiza(InstructorModel parametros)
        {
            throw new NotImplementedException();
        }

        public Task<int> Elimina(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<int> Nuevo(InstructorModel parametros)
        {
            throw new NotImplementedException();
        }

        public Task<IList<InstructorModel>> ObtenerLista()
        {
            throw new NotImplementedException();
        }

        public Task<InstructorModel> ObtenerPorId(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}