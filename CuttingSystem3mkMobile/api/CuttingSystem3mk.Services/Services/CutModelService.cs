using CuttingSystem3mk.Models.Models;
using CuttingSystem3mk.Repositories;
using CuttingSystem3mk.Repositories.Repositories;

namespace CuttingSystem3mk.Services.Services
{
    public class CutModelService : BaseService<CutModelService, CutModelRepository, CutModel>
    {
        public CutModelService
            (
                BaseRepository<CutModel, CutModelRepository> baseRepository,
                Repository repository
            ) : base(baseRepository, repository)
        {
            
        }
    }
}
