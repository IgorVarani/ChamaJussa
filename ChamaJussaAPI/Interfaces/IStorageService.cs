using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace ChamaJussaAPI.Interfaces
{
    public interface IStorageService
    {
        Task<string?> UploadImagemAsync(IFormFile arquivo);
    }
}
