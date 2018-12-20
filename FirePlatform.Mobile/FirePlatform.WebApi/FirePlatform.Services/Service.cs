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
        private readonly FormService _formService;

        public Service
            (
                UserService userService,
                FormService formService
            )
        {
            _userService = userService;
            _formService = formService;
        }

        #region Methods

        public UserService GetUserService()
        {
            return _userService;
        }

        public FormService GetFormService()
        {
            return _formService;
        }

        #endregion
    }
}
