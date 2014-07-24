using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.Entity;
using PreScripds.Infrastructure;

namespace PreScripds.DAL
{
    public class PreScripdsConfiguration : DbConfiguration
    {
        public PreScripdsConfiguration()
        {
            //SetExecutionStrategy("MySql.Data.MySqlClient", () => new MySqlExecutionStrategy());
            if (ConfigurationManager.AppSettings["CreateDbIfNotExists"].AsBool(false))
            {
                SetDatabaseInitializer(new PreScripdsDbIntializer());
            }
        }

    }
}
