using FirePlatform.Models.Models;
using FirePlatform.Repositories;
using FirePlatform.Repositories.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace FirePlatform.Services.Services
{
    public class UserTemplatesService : BaseService<UserTemplatesService, UserTemplatesRepository, UserTemplates>
    {
        public UserTemplatesService
            (
                BaseRepository<UserTemplates, UserTemplatesRepository> baseRepository,
                Repository repository
            ) : base(baseRepository, repository)
        {
        }
        public (bool success, string error) Save(User user, string mainTmp, string nameTmp, byte[] data)
        {
            try
            {
                if (user == null) return (false, "User is null");
                var context = Repository.GetUserTemplatesRepository();
                var newTemplate = new UserTemplates()
                {
                    Id = 5,
                    Name = nameTmp,
                    MainName = mainTmp,
                    User = user,
                    UserId = user.Id,
                    Data = data
                };
                 context.Create(newTemplate);
                return (true, null);
            }
            catch(Exception ex)
            {
                return (false, ex.Message);
            }
        }
        public IList<(int id, string name)> GetNameTemplates(User user)
        {
            try
            {
                if (user == null) return null;
                var context = Repository.GetUserTemplatesRepository();
                var templates = context.Get(x => x.UserId == user.Id);
                var result = templates.Select(x => (x.Id, x.Name)).ToList();
                return result;
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        public (bool success, string error) Delete(User user, int templateId)
        {
            try
            {
                if (user == null) return (false, "User is null");
                var context = Repository.GetUserTemplatesRepository();
                context.Delete(templateId);
                return (true, null);
            }
            catch(Exception ex)
            {
                return (false, ex.Message);
            }
        }
    }
}
