using FirePlatform.Models.Models;
using FirePlatform.Repositories;
using FirePlatform.Repositories.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace FirePlatform.Services.Services
{
    public class MainTemplatesService : BaseService<MainTemplatesService, MainTemplatesRepository, MainTemplates>
    {
        public MainTemplatesService
           (
               BaseRepository<MainTemplates, MainTemplatesRepository> baseRepository,
               Repository repository
           ) : base(baseRepository, repository)
        {
        }

        public (bool success, string error) Save(MainTemplates request)
        {
            try
            {
                if (request == null)
                    return (false, "Request template is empty");
                var context = Repository.GetMainTemplatesRepository();
                context.Create(request);
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public MainTemplates TryGetTemplateOrDefault(MainTemplates request)
        {
            MainTemplates result = null;
            try
            {
                if (request != null)
                {
                    var context = Repository.GetMainTemplatesRepository();
                    result = context.Get(x => x.Lng == request.Lng && x.LongName == request.LongName && x.ShortName == request.ShortName)?.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        public bool RemoveAll(int id)
        {
            try
            {
                var context = Repository.GetMainTemplatesRepository();
                context.Delete(id);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
    }
}
