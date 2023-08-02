using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Project.FileUploadService
{
    public interface IFileUploadService
    {
        Task<string> UploadFileAsync(IFormFile file);
    }
}
