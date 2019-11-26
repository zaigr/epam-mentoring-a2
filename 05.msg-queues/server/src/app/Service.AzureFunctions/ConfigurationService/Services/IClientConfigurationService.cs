using System.Threading.Tasks;

namespace Service.AzureFunctions.ConfigurationService.Services
{
    public interface IClientConfigurationService
    {
        Task UpdateClientConfigurationAsync();
    }
}
