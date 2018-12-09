using FirePlatform.Models.Models;
using FirePlatform.Services.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace FirePlatform.Services
{
    public class Service
    {
        private readonly UserService _userService;

        public Service
            (
                UserService userService
            )
        {
            _userService = userService;
        }

        #region Methods

        public UserService GetUserService()
        {
            return _userService;
        }

        #endregion
    }
}
