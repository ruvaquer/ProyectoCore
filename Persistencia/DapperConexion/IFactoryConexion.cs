using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Persistencia.DapperConexion
{
    public interface IFactoryConexion
    {
        void CloseConnection();
        IDbConnection GetConnection();
    }
}