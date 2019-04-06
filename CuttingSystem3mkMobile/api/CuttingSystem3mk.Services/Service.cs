using CuttingSystem3mk.Services.Services;

namespace CuttingSystem3mk.Services
{
    public class Service
    {
        private readonly UserService _userService;
        private readonly CutModelService _cutModelService;

        public Service
            (
                UserService userService,
                CutModelService cutModelService
            )
        {
            _userService = userService;
            _cutModelService = cutModelService;
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
        #endregion
    }
}
