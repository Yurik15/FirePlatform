using CuttingSystem3mk.Models.Containers;
using CuttingSystem3mk.Models.Models;
using CuttingSystem3mk.Repositories;
using CuttingSystem3mk.Repositories.Repositories;
using CuttingSystem3mk.Utils.AlgorithmHelpers;
using System.Linq;
using System.Threading.Tasks;

namespace CuttingSystem3mk.Services.Services
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
