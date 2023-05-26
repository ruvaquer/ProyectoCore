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
            try
            {
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
            }
            catch (Exception ex)
            {
                throw new Exception("Error en la consulta de datos actualizar instructor " + ex.StackTrace);
            }
            finally
            {
                _factoryConnection.CloseConnection();
            }

            //}

            return resultado;
        }

        public async Task<int> Elimina(Guid id)
        {
            var storeProcedure = "usp_intructor_eliminar";
            int resultado = 0;
            var connection = _factoryConnection.GetConnection();
            //using(var transaction = connection.BeginTransaction() ){
            try
            {
                resultado = await connection.ExecuteAsync(storeProcedure,
                new
                {
                    IntructorId = id
                },
                    commandType: CommandType.StoredProcedure
                );
                //transaction.Commit();
            }
            catch (Exception ex)
            {
                throw new Exception("Error no se pudo eliminar eliminar instructor " + ex.StackTrace);
            }
            finally
            {
                _factoryConnection.CloseConnection();
            }

            //}

            return resultado;
        }

        public async Task<int> Nuevo(InstructorModel parametros)
        {
            var storeProcedure = "usp_intructor_nuevo";
            int resultado = 0;

            var connection = _factoryConnection.GetConnection();
            //using (var transaction = connection.BeginTransaction())
            //{
            try
            {
                resultado = await connection.ExecuteAsync(storeProcedure, new
                {
                    IntructorId = Guid.NewGuid(),
                    Nombre = parametros.Nombre,
                    Apellidos = parametros.Apellidos,
                    Grado = parametros.Grado
                },
                    commandType: CommandType.StoredProcedure
                );
                //transaction.Commit();
            }
            catch (Exception ex)
            {
                throw new Exception("Error en la consulta de datos nuevo instructor " + ex.StackTrace);
            }
            finally
            {
                _factoryConnection.CloseConnection();
            }

            //}

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

        public async Task<InstructorModel> ObtenerPorId(Guid id)
        {
            InstructorModel intructor = null;
            var storeProcedure = "usp_obtener_intructor_por_id";
            try
            {
                var connection = _factoryConnection.GetConnection();
                //Le meto dentro del QueryFirstAsync el objeto para mapear dicho objeto antes de retornarlo
                intructor = await connection.QueryFirstAsync<InstructorModel>(
                    storeProcedure,
                    new
                    {
                        Id = id
                    },
                    commandType: CommandType.StoredProcedure
                    );
                

            }
            catch (Exception ex)
            {
                throw new Exception("Error al recuperar el instructor con Id " + id + ": Error--> ", ex);
            }
            finally
            {
                _factoryConnection.CloseConnection();
            }
            return intructor;
        }
    }
}