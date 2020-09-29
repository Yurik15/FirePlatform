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
        private readonly UserTemplatesService _userTemplatesService;
        private readonly MainTemplatesService _mainTemplatesService;
        private readonly ScriptDefinitionService _scriptDefinitionService;

        public Service
            (
                UserService userService,
                FormService formService,
                UserTemplatesService userTemplatesService,
                MainTemplatesService mainTemplatesService,
                ScriptDefinitionService scriptDefinitionService
            )
        {
            _userService = userService;
            _formService = formService;
            _userTemplatesService = userTemplatesService;
            _mainTemplatesService = mainTemplatesService;
            _scriptDefinitionService = scriptDefinitionService;
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

        public UserTemplatesService GetUserTemplatesService()
        {
            return _userTemplatesService;
        }

        public MainTemplatesService GetMainTemplatesService()
        {
            return _mainTemplatesService;
        }
        public ScriptDefinitionService GetScriptDefinitionService()
        {
            return _scriptDefinitionService;
        }
        #endregion
    }
}
