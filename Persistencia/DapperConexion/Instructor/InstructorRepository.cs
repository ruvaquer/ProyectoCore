using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

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

        public async Task<List<InstructorModel>> ObtenerLista()
        {
            IEnumerable<InstructorModel> intructorList = null;
            var storeProcedure = "usp_Obtener_Instructores";
            try{
                var connection = _factoryConnection.GetConnection();
                intructorList =  await connection.QueryAsync<InstructorModel>(storeProcedure,null,commandType : CommandType.StoredProcedure);
            }catch(Exception ex){
                throw new Exception("Error en la consulta de datos obtener lista de instructores ",ex);
            }finally{
                _factoryConnection.CloseConnection();
            }
            return intructorList.ToList();
        }

        public Task<InstructorModel> ObtenerPorId(Guid id)
        {
             throw new NotImplementedException();
        }
    }
}