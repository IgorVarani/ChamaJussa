using Microsoft.AspNetCore.Http;
using System;

namespace ChamaJussaAPI.DTOs.OrdemServicoDto
{
    public class EditarOrdemServicoDto
    {
        public string? NomeItem { get; set; }
        public int? LocalizacaoId { get; set; }
        public string? Descricao { get; set; }
        public IFormFile? Imagem { get; set; }
    }
}
