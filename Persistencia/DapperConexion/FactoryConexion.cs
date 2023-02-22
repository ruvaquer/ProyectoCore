using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace Persistencia.DapperConexion
{
    public class FactoryConexion : IFactoryConexion
    {
        private IDbConnection _connection;
        private readonly IOptions<ConexionConfiguracion> _configs;//Así le pasamos al configuración de conexión, que ya se ha implementado en el startUp de WebApi

        public FactoryConexion(IDbConnection connection){
            _connection = connection;//Inyectamos la conexion
        }

        ///Importante siempre Cerrar todas las conexiones según se van usando en la llamadas a la BBDD Finnaly recomendado poner siempre
        public void CloseConnection()
        {
            if(_connection != null && _connection.State == ConnectionState.Open){
                _connection.Close();
            }
        }

        public IDbConnection GetConnection()
        {
            if(_connection == null){
                //Aqui se crea la cadena de conexión con la configuración aplicada en StartUp del proyecto WebApi
                _connection = new SqlConnection(_configs.Value.ConexionSql);
            }
            if(_connection.State != ConnectionState.Open){
                _connection.Open();
            }
            return _connection;
        }
    }
}