using FirePlatform.Repositories.Repositories;

namespace FirePlatform.Repositories
{
    public class Repository
    {
        public static UserRepository GetUserRepository()
        {
            return UserRepository.Instance;
        }
    }
}
