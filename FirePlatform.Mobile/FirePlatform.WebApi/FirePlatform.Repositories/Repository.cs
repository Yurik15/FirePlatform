using FirePlatform.Repositories.Repositories;

namespace FirePlatform.Repositories
{
    public class Repository
    {
        public static UserRepository GetUserRepository()
        {
            return UserRepository.Instance;
        }
        public static FormRepository GetFormRepository()
        {
            return FormRepository.Instance;
        }
        public static UserFormRepository GetUserFormRepository()
        {
            return UserFormRepository.Instance;
        }
    }
}
