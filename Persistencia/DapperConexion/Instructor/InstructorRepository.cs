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
        public InstructorRepository(IFactoryConexion factoryConnection)
        {
            _factoryConnection = factoryConnection;
        }

        public async Task<int> Actualiza(InstructorModel parametros)
        {
            var storeProcedure = "usp_intructor_editar";
            int resultado = 0;
            var connection = _factoryConnection.GetConnection();
            //using(var transaction = connection.BeginTransaction() ){
                try{
                    resultado = await connection.ExecuteAsync(storeProcedure, 
                    new
                    {
                        IntructorId = parametros.InstructorId,
                        Nombre = parametros.Nombre,
                        Apellidos = parametros.Apellidos,
                        Grado = parametros.Grado
                    },
                        commandType: CommandType.StoredProcedure
                    );
                    //transaction.Commit();
                }catch(Exception ex){
                    throw new Exception("Error en la consulta de datos actualizar instructor "+ex.StackTrace);
                }finally
                {
                    _factoryConnection.CloseConnection();
                }
                    
            //}
              
            return resultado;
        }

        public Task<int> Elimina(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<int> Nuevo(InstructorModel parametros)
        {
            var storeProcedure = "usp_intructor_nuevo";
            int resultado = 0;
            
                var connection = _factoryConnection.GetConnection();
                using(var transaction = connection.BeginTransaction() ){
                    try{
                        resultado = await connection.ExecuteAsync(storeProcedure, new
                        {
                            IntructorId = Guid.NewGuid(),
                            Nombre = parametros.Nombre,
                            Apellidos = parametros.Apellidos,
                            Grado = parametros.Grado
                        },
                            commandType: CommandType.StoredProcedure
                        );
                        transaction.Commit();
                    }catch(Exception ex){
                        throw new Exception("Error en la consulta de datos nuevo instructor "+ex.StackTrace);
                    }finally
                    {
                        _factoryConnection.CloseConnection();
                    }
                    
                }
              
            return resultado;
        }

        public async Task<List<InstructorModel>> ObtenerLista()
        {
            IEnumerable<InstructorModel> intructorList = null;
            var storeProcedure = "usp_Obtener_Instructores";
            try
            {
                var connection = _factoryConnection.GetConnection();
                intructorList = await connection.QueryAsync<InstructorModel>(storeProcedure, null, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                throw new Exception("Error en la consulta de datos obtener lista de instructores ", ex);
            }
            finally
            {
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