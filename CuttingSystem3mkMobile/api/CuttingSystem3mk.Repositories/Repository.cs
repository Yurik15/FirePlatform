using CuttingSystem3mk.Repositories.Repositories;

namespace CuttingSystem3mk.Repositories
{
    public class Repository
    {
        public static UserRepository GetUserRepository()
        {
            return UserRepository.Instance;
        }
        public static CutModelRepository CutModelRepository()
        {
            return Repositories.CutModelRepository.Instance;
        }
        public static DeviceModelRepository DeviceModelRepository()
        {
            return Repositories.DeviceModelRepository.Instance;
        }
    }
}
