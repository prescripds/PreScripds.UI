using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PreScripds.Domain;

namespace PreScripds.DAL.Interface
{
    public interface IMasterRepository
    {
        ICollection<Department> GetDepartments();

        ICollection<Role> GetRoles();
    }
}
