using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Threading.Tasks;

namespace Project.FileUploadService
{
    public class LocalFileUploadService : IFileUploadService
    {
        private readonly IHostEnvironment _env;

        public LocalFileUploadService(IHostEnvironment env)
        {
            _env = env;
        }

        public async Task<string> UploadFileAsync(IFormFile file)
        {
            var filePath = Path.Combine(_env.ContentRootPath, @"wwwroot\images", file.FileName);
            using var fileStream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(fileStream);
            return filePath;
        }
    }
}
