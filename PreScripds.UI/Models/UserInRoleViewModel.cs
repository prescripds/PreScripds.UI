using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PreScripds.Domain;

namespace PreScripds.UI.Models
{
    public class UserInRoleViewModel
    {
        public long Id { get; set; }
        public long RoleId { get; set; }
        public List<Role> Roles { get; set; }
        public IEnumerable<string> UsersSelected { get; set; }
        public List<User> Users { get; set; }
        public bool IsActive { get; set; }
        public string RoleName { get; set; }
        public string UserName { get; set; }
        public List<UserInRoleViewModel> UserInRoleViewModels { get; set; }
        public bool CreationSuccessful { get; set; }
        public string Message { get; set; }
    }
}