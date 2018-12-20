using Refit;
using System.Threading.Tasks;
using FirePlatform.Mobile;
using FirePlatform.Mobile.Common.Models;

namespace FirePlatform.Mobile.Common.Interfaces.Communication
{
    public interface IAccountApi
    {
        #region Authorize

        [Post("/api/Account/Login")]
        Task<UserCredentials> Login([Body] UserCredentials userCredentials);

        [Post("/api/Account/Register")]
        Task<UserCredentials> Register([Body] UserCredentials userCredentials);

        #endregion
    }
}
