using System;
using System.Threading.Tasks;

namespace CuttingSystem3mkMobile.Services
{
    public interface IQrScanningService
    {
        Task<string> ScanAsync();
    }
}
