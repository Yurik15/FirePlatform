using CuttingSystem3mk.Repositories.Repositories;
using CuttingSystem3mk.Utils.AlgorithmHelpers;
using CuttingSystem3mkMobile.Models.Containers;
using CuttingSystem3mkMobile.Models.Models;
using CuttingSystem3mkMobile.Repositories;
using CuttingSystem3mkMobile.Repositories.Repositories;
using CuttingSystem3mkMobile.Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CuttingSystem3mk.Services.Services
{

    public class CutCodeService : BaseService<CutCodeService, CutCodesRepository, CutCode>
    {
        public CutCodeService
            (
                BaseRepository<CutCode, CutCodesRepository> baseRepository,
                Repository repository
            ) : base(baseRepository, repository)
        {

        }

        public async Task<bool?> CheckCodes(string code)
        {
            var codeFromDb = (await Repository.GetCutCodesRepository().Get(x => x.Barcode == code)).FirstOrDefault();

            if (String.IsNullOrEmpty(codeFromDb.Barcode))
                return null;

            if (!codeFromDb.IsActive)
                return false;

            else
            {
                codeFromDb.IsActive = false;
                await Repository.GetCutCodesRepository().Update(codeFromDb);

                return true;
            }
        }

        public async Task<List<CutCode>> GenerateCodes(int codeCount, int charsCount)
        {
            var generatedCodes = new List<CutCode>();
            for (int i = 0; i < codeCount; i++)
            {
                var generatedCode = Generator.RandomString(charsCount);
                var sampleCodeFromDb = Repository.GetCutCodesRepository().GetIQueryable(x => x.Barcode == generatedCode);
                if (sampleCodeFromDb.Any())
                    await GenerateCodes(codeCount, charsCount);
                else
                    generatedCodes.Add
                    (
                        new CutCode()
                        {
                            Barcode = generatedCode,
                            IsActive = true
                        }
                    );
            }
            await Repository.GetCutCodesRepository().CreateRange(generatedCodes);

            return generatedCodes;

        }
    }
}
