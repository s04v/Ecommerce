using Common.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Storage
{
    public class LocalStorageService : IStorageService
    {
        private readonly string _rootPath;

        public LocalStorageService(IConfiguration config)
        {
            _rootPath = config.GetValue<string>("FileStorage:LocalRootPath");
        }

        public string GetFileUrl(string fileName)
        {
            return $"/{_rootPath}/{fileName}";
        }

        public async Task SaveFileAsync(Stream stream, string fileName)
        {
            var filePath = Path.Combine(_rootPath, fileName);
            using (var output = new FileStream(filePath, FileMode.Create))
            {
                await stream.CopyToAsync(output);
            }
        }

        public async Task DeleteFileAsync(string fileName)
        {
            var filePath = Path.Combine(_rootPath, fileName);
            if (File.Exists(filePath))
            {
                await Task.Run(() => File.Delete(filePath));
            }
        }
    }
}
