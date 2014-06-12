using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PreScripds.DAL;

namespace PreScripds.Infrastructure.UnitOfWork
{
    public class UnitOfWork : BaseUnitOfWork
    {
        public UnitOfWork()
            : base(new PreScripdsDb())
        {

        }
    }
}
