using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace EventApp.Data.Sql
{
    public interface IDbConnectionFactory
    {
        Task<IDbConnection> OpenConnection();
    }
}
