using CuttingSystem3mk.Services.Services;
using CuttingSystem3mkMobile.Services.Services;

namespace CuttingSystem3mkMobile.Services
{
    public class Service
    {
        private readonly UserService _userService;
        private readonly CutModelService _cutModelService;
        private readonly DeviceModelService _deviceModelService;
        private readonly CutCodeService _cutCodeService;

        public Service
            (
                UserService userService,
                CutModelService cutModelService,
                DeviceModelService deviceModelService,
                CutCodeService cutCodeService
            )
        {
            _userService = userService;
            _cutModelService = cutModelService;
            _deviceModelService = deviceModelService;
            _cutCodeService = cutCodeService;
        }

        #region Methods

        public UserService GetUserService()
        {
            return _userService;
        }
        public CutModelService GetCutModelService()
        {
            return _cutModelService;
        }
        public DeviceModelService GetDeviceModelService()
        {
            return _deviceModelService;
        }
        public CutCodeService GetCutCodeService()
        {
            return _cutCodeService;
        }

        #endregion
    }
}
