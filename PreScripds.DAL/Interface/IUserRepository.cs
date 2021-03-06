﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PreScripds.Domain;
using PreScripds.Domain.Enums;
using PreScripds.Infrastructure.Repositories;

namespace PreScripds.DAL.Interface
{
    public interface IUserRepository
    {
        List<User> GetUsers(long organzationId);
        User AddUser(User user);
        Role AddRole(Role role);
        User GetUserByUsername(string loginName, LoginType loginType);
        User CheckEmailExists(string email);
        List<Role> GetRole(long organizationId);
        bool CheckRoleExists(Role role);
        List<Department> GetAllDepartment();
        Organization AddOrganization(Organization organization);
        bool CheckOrganizationExist(string orgName);
        UserHistory AddUserHistory(UserHistory userHistory);
        void UpdateUserLogin(UserHistory userHistory);
        void UpdateUserByAdmin(long id, bool isActive);
        List<Department> GetDepartmentInOrganization(long organizationId);
        User GetUserById(long Id);
        User UpdateUserProfile(User user);
        UserLogin GetUserLoginById(long id);
        string ChangePassword(UserLogin userlogin);
        string ChangeSecurityAnswer(UserLogin userLogin);
    }
}
