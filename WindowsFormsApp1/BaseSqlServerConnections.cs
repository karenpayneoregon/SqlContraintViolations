using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    public class BaseSqlServerConnections : BaseExceptionsHandler
    {
        /// <summary>
        /// This points to your database server
        /// </summary>
        protected string DatabaseServer = "KARENS-PC";
        /// <summary>
        /// Name of database containing required tables
        /// </summary>
        protected string DefaultCatalog = "";
        public string ConnectionString => $"Data Source={DatabaseServer};Initial Catalog={DefaultCatalog};Integrated Security=True";
    }
}
