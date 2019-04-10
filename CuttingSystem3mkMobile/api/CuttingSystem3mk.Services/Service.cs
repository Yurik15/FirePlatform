using CuttingSystem3mkMobile.Services.Services;

namespace CuttingSystem3mkMobile.Services
{
    public class Service
    {
        private readonly UserService _userService;
        private readonly CutModelService _cutModelService;
        private readonly DeviceModelService _deviceModelService;

        public Service
            (
                UserService userService,
                CutModelService cutModelService,
                DeviceModelService deviceModelService
            )
        {
            _userService = userService;
            _cutModelService = cutModelService;
            _deviceModelService = deviceModelService;
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
        #endregion
    }
}
