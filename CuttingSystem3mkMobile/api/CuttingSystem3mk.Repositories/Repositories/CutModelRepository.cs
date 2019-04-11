using CuttingSystem3mkMobile.Models.Models;
using CuttingSystem3mkMobile.Repositories.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CuttingSystem3mkMobile.Repositories.Repositories
{
    public class CutModelRepository : BaseRepository<CutModel, CutModelRepository>
    {
        public CutModelRepository() : base() { }

        public async Task<List<CutModel>> GetByDeviceIdIncludeFiles(int id)
        {
            return await _context.Set<CutModel>()
                        .AsNoTracking()
                        .Include(x => x.CutFile)
                        .Where(x => x.IdDeviceModel == id)
                        .ToListAsync();
        }

        public async Task<List<CutModel>> GetAllIncludeFiles()
        {
            return await _context.Set<CutModel>()
                        .AsNoTracking()
                        .Include(x => x.CutFile)
                        .ToListAsync();
        }
    }
}
