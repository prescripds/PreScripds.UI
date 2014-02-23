using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PreScripds.Domain;

namespace PreScripds.DAL
{
    public class PreScripdsDb : DbContext
    {
        static PreScripdsDb()
        {
            Database.SetInitializer<PreScripdsDb>(null);
        }
        public PreScripdsDb()
            : base("Name=PreScripdsDb")
        {
        }

        public DbSet<Department> departments { get; set; }
        public DbSet<Role> roles { get; set; }
        public DbSet<User> users { get; set; }


    }
}
