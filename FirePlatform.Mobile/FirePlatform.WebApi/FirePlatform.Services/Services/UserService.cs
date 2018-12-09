using FirePlatform.Models.Containers;
using FirePlatform.Models.Models;
using FirePlatform.Repositories;
using FirePlatform.Repositories.Repositories;
using FirePlatform.Utils.AlgorithmHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirePlatform.Services.Services
{
    public class UserService : BaseService<UserService, UserRepository, User>
    {
        public UserService
            (
                BaseRepository<User, UserRepository> baseRepository,
                Repository repository
            ) : base(baseRepository, repository)
        {

        }

        public bool ValidUserCredentials(string login, string password)
        {
            var hash = EncodeAlgorithms.ComputeSha256Hash(password);
            var userFromDb = Repository.GetUserRepository().GetIQueryable(x => x.Login == login && x.Password == hash);

            if (userFromDb.Any())
                return true;
            else
                return false;
        }

        public async Task<ServiceContainer<User>> Register(User user)
        {
            var container = new ServiceContainer<User>();

            var userFromDb = Repository.GetUserRepository().GetIQueryable(x => x.Login == user.Login);
            if (!userFromDb.Any())
            {
                user.Password = EncodeAlgorithms.ComputeSha256Hash(user.Password);
                var result = await Repository.GetUserRepository().Create(user);
                container.DataObject = result;

                return container;
            }
            else
            {
                container.DataObject = user;
                container.Message = "User with same login is already exists";

                return container;
            }
        }
    }
}
