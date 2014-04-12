﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PreScripds.DAL.Interface;
using PreScripds.Domain;
using PreScripds.Infrastructure.Repositories;
using System.Data.Entity;
using PreScripds.Infrastructure.Security;
using PreScripds.Infrastructure;
using MySql.Data.MySqlClient;

namespace PreScripds.DAL.Repository
{
    public class UserRepository : RepositoryBase<User, PreScripdsDb>, IUserRepository
    {
        private readonly PreScripdsDb _dbContext;
        public UserRepository(PreScripdsDb context)
            : base(context)
        {
            _dbContext = context;
        }
        public async Task<List<User>> GetUsers()
        {
            var userLst = await ContextRep.users.Include(x => x.UserLogin).ToListAsync();
            return userLst;
        }

        public User AddUser(User user)
        {
            if (user.UserLogin.IsCollectionValid())
            {
                foreach (var userLogin in user.UserLogin)
                {
                    var saltKey = EncryptionExtensions.CreateSaltKey();
                    userLogin.SaltKey = saltKey;
                    var encryptedPassword = EncryptionExtensions.CreatePasswordHash(userLogin.Password,
                                userLogin.SaltKey);
                    userLogin.Password = encryptedPassword;
                    var encryptedSecurityAnswer = EncryptionExtensions.CreatePasswordHash(userLogin.SecurityAnswer, userLogin.SaltKey);
                    userLogin.SecurityAnswer = encryptedSecurityAnswer;
                }
            }
            ContextRep.users.Add(user);
            ContextRep.SaveChangesAsync();
            return user;
        }

        public async Task<User> GetUserByUsername(string loginName)
        {
            var users = await ContextRep.users.Include(x => x.UserLogin).Where(x => x.Active == 1).ToListAsync();
            if (users.IsCollectionValid())
            {
                var loginUser = users.Where(x => x.UserLogin.First().UserName == loginName);
                if (loginUser != null)
                    return loginUser.FirstOrDefault();
            }
            return null;
        }

        public async Task<User> CheckEmailExists(string email)
        {
            var users = await ContextRep.users.Include(x => x.UserLogin).Where(x => x.Active == 1 && x.Email == email).ToListAsync();
            if (users.IsCollectionValid())
            {
                return users.First();
            }
            return null;
        }
    }
}
