using Microsoft.AspNetCore.Http;
using System;

namespace ChamaJussaAPI.DTOs.OrdemServicoDto
{
    public class CriarOrdemServicoDto
    {
        public string NomeItem { get; set; } = string.Empty;
        public int? LocalizacaoId { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public IFormFile? Imagem { get; set; }
    }
}
