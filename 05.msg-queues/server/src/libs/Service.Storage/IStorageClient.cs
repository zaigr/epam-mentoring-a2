using System.IO;
using System.Threading.Tasks;

namespace Service.Storage
{
    public interface IStorageClient
    {
        Task CreateOrReplaceAsync(string fileName);

        Task AppendAsync(string fileName, byte[] content);

        Task<bool> ExistsAsync(string fileName);
    }
}
