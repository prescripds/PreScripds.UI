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

        public async Task<List<User>> GetUsers()
        {
            var users = await _userRepository.GetUsers();
            return users;
        }

        public User AddUser(User user)
        {
            var userFromDb = _userRepository.AddUser(user);
            return userFromDb;
        }

        public Task<User> GetUserByUsername(string loginName)
        {
            var user = _userRepository.GetUserByUsername(loginName);
            return user;
        }

        public Task<User> CheckEmailExists(string email)
        {
            var user = _userRepository.CheckEmailExists(email);
            return user;
        }
    }
}
