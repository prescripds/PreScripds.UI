using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PreScripds.Domain;
using PreScripds.Domain.Master;

namespace PreScripds.DAL.Interface
{
    public interface IMasterRepository
    {
        ICollection<Department> GetDepartments();

        ICollection<Permission> GetPermission();
        ICollection<Country> GetCountry();
        ICollection<State> GetState();
        ICollection<SecurityQuestion> GetSecurityQuestion();
        ICollection<Module> GetModule();
    }
}
