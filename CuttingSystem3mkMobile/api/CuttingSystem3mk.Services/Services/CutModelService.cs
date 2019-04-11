using CuttingSystem3mk.Utils.AlgorithmHelpers;
using CuttingSystem3mkMobile.Models.Containers;
using CuttingSystem3mkMobile.Models.Models;
using CuttingSystem3mkMobile.Repositories;
using CuttingSystem3mkMobile.Repositories.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CuttingSystem3mkMobile.Services.Services
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

        public async Task<ServiceContainer<CutModel>> GetByDeviceId(int idDevice)
        {
            var cutModels = await Repository.GetCutModelRepository().GetByDeviceIdIncludeFiles(idDevice);
            cutModels = DecodeFiles(cutModels);

            return new ServiceContainer<CutModel>()
            {
                DataCollection = cutModels
            };
        }

        public async Task<ServiceContainer<CutModel>> SynchronizeFileNames(Dictionary<string, byte[]> fileNameDict)
        {
            var cutModels = await Repository.GetCutModelRepository().GetAllIncludeFiles();

            foreach (var fileNameEl in fileNameDict)
            {
                foreach (var cutModel in cutModels)
                {
                    if (cutModel.Name.Equals(fileNameEl.Key))
                    {
                        string utfString = Convert.ToBase64String(fileNameEl.Value);
                        var encryptedFile = EncryptionHelper.Encrypt(utfString);
                        cutModel.CutFile.ImageValue = encryptedFile;

                        await Repository.GetCutModelRepository().Update(cutModel);
                    }
                }
            }

            return new ServiceContainer<CutModel>()
            {
                DataCollection = cutModels
            };
        }

        public async Task SynchronizeAddFilesToDb(Dictionary<string, byte[]> files)
        {
            foreach (var file in files)
            {
                string utfString = Convert.ToBase64String(file.Value);
                var encryptedFile = EncryptionHelper.Encrypt(utfString);

                DeviceModel deviceModel = new DeviceModel()
                {
                    Name = file.Key,
                    CutModels = new List<CutModel>()
                    {
                        new CutModel()
                        {
                            Name = file.Key,
                            CutFile = new CutFile()
                            {
                                Value = encryptedFile
                            }
                        }
                    }
                };
                await Repository.GetDeviceModelRepository().Create(deviceModel);
            }
        }



        private List<CutModel> DecodeFiles(List<CutModel> cutModels)
        {
            foreach (var cutModel in cutModels)
            {
                cutModel.CutFile.Value = EncryptionHelper.Decrypt(cutModel.CutFile.Value);
                cutModel.CutFile.ImageValue = EncryptionHelper.Decrypt(cutModel.CutFile.ImageValue);
            }
            return cutModels;
        }
    }
}