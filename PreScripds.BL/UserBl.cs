﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PreScripds.BL.Interface;
using PreScripds.DAL;
using PreScripds.DAL.Interface;
using PreScripds.DAL.Repository;
using PreScripds.Domain;
using PreScripds.Domain.Enums;

namespace PreScripds.BL
{
    public class UserBl : IUserBl
    {
        private IUserRepository _userRepository;
        //private PreScripdsDb context;
        public UserBl(PreScripdsDb context)
        {
            _userRepository = new UserRepository(context);
        }

        public List<User> GetUsers(long organzationId)
        {
            var users = _userRepository.GetUsers(organzationId);
            return users;
        }

        public User AddUser(User user)
        {
            var userFromDb = _userRepository.AddUser(user);
            //TODO:Send mail to User about successful login.
            return userFromDb;
        }

        public Role AddRole(Role role)
        {
            var roleFromDb = _userRepository.AddRole(role);
            return roleFromDb;
        }

        public User GetUserByUsername(string loginName, LoginType loginType)
        {
            var user = _userRepository.GetUserByUsername(loginName, loginType);
            return user;
        }

        public User CheckEmailExists(string email)
        {
            var user = _userRepository.CheckEmailExists(email);
            return user;
        }

        public bool CheckRoleExists(Role role)
        {
            var roleModel = _userRepository.CheckRoleExists(role);
            return roleModel;
        }
        public List<Role> GetRole(long organizationId)
        {
            var roles = _userRepository.GetRole(organizationId);
            return roles;
        }
        public List<Department> GetAllDepartment()
        {
            var departments = _userRepository.GetAllDepartment();
            return departments;
        }
        public Organization AddOrganization(Organization organization)
        {
            var organizations = _userRepository.AddOrganization(organization);
            //TODO:Send Email to Prescripds Super Admin
            return organizations;
        }

        public bool CheckOrganizationExist(string orgName)
        {
            var isExist = _userRepository.CheckOrganizationExist(orgName);
            return isExist;
        }

        public UserHistory AddUserHistory(UserHistory userHistory)
        {
            var userHistry = _userRepository.AddUserHistory(userHistory);
            return userHistry;
        }

        public void UpdateUserLogin(UserHistory userHistory)
        {
            _userRepository.UpdateUserLogin(userHistory);
        }

        public void UpdateUserByAdmin(long id, bool isActive)
        {
            _userRepository.UpdateUserByAdmin(id, isActive);
        }

        public List<Department> GetDepartmentInOrganization(long organizationId)
        {
            var departments = _userRepository.GetDepartmentInOrganization(organizationId);
            return departments;
        }
        public User GetUserById(long Id)
        {
            var user = _userRepository.GetUserById(Id);
            return user;
        }
        public User UpdateUserProfile(User user)
        {
            var userPfl = _userRepository.UpdateUserProfile(user);
            return userPfl;
        }
        public UserLogin GetUserLoginById(long id)
        {
            var userLogin = _userRepository.GetUserLoginById(id);
            return userLogin;
        }
        public string ChangePassword(UserLogin userlogin)
        {
            var userLogin = _userRepository.ChangePassword(userlogin);
            return userLogin;
        }
        public string ChangeSecurityAnswer(UserLogin userlogin)
        {
            var userLogin = _userRepository.ChangeSecurityAnswer(userlogin);
            return userLogin;
        }
    }
}
