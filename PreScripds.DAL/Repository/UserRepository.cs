using System;
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
        public List<User> GetUsers()
        {
            var userLst = ContextRep.users.Include(x => x.UserLogin).ToList();
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
            ContextRep.SaveChanges();
            return user;
        }
    }
}
