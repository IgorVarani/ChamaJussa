using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using ChamaJussaAPI.Interfaces;

namespace ChamaJussaAPI.Applications.Services
{
    public class LocalStorageService : IStorageService
    {
        private readonly IWebHostEnvironment _env;

        public LocalStorageService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task<string?> UploadImagemAsync(IFormFile arquivo)
        {
            if (arquivo == null || arquivo.Length == 0)
            {
                return null;
            }

            // Define a pasta wwwroot/uploads
            string rootPath = _env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            string uploadsFolder = Path.Combine(rootPath, "uploads");

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            // Gera um nome único para o arquivo local
            var extensao = Path.GetExtension(arquivo.FileName);
            var nomeArquivo = $"os-{Guid.NewGuid()}{extensao}";
            var caminhoCompleto = Path.Combine(uploadsFolder, nomeArquivo);

            using (var stream = new FileStream(caminhoCompleto, FileMode.Create))
            {
                await arquivo.CopyToAsync(stream);
            }

            // Retorna o caminho relativo estático (ex: /uploads/os-xxxx.jpg)
            return $"/uploads/{nomeArquivo}";
        }
    }
}
