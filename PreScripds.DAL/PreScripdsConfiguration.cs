using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.Entity;

namespace PreScripds.DAL
{
    public class PreScripdsConfiguration : DbConfiguration
    {
        public PreScripdsConfiguration()
        {
            //SetExecutionStrategy("MySql.Data.MySqlClient", () => new MySqlExecutionStrategy());
            SetDatabaseInitializer(new CreateDatabaseIfNotExists<PreScripdsDb>());
        }

    }
}
