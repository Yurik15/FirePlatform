using CuttingSystem3mk.Repositories.Repositories;
using CuttingSystem3mkMobile.Repositories.Repositories;

namespace CuttingSystem3mkMobile.Repositories
{
    public class Repository
    {
        public static UserRepository GetUserRepository()
        {
            return UserRepository.Instance;
        }
        public static CutModelRepository GetCutModelRepository()
        {
            return Repositories.CutModelRepository.Instance;
        }
        public static DeviceModelRepository GetDeviceModelRepository()
        {
            return Repositories.DeviceModelRepository.Instance;
        }
        public static CutCodesRepository GetCutCodesRepository()
        {
            return CutCodesRepository.Instance;
        }
    }
}
