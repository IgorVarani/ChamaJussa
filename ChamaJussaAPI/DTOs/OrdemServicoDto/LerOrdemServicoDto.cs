using System;

namespace ChamaJussaAPI.DTOs.OrdemServicoDto
{
    public class LerOrdemServicoDto
    {
        public int OsId { get; set; }
        public string NomeItem { get; set; } = string.Empty;
        public Guid? Solicitante { get; set; }
        public string? SolicitanteNome { get; set; }
        public DateTime DtCriacao { get; set; }
        public int? LocalizacaoId { get; set; }
        public string? LocalizacaoNome { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public string? Imagem { get; set; }
        public int? StatusId { get; set; }
        public string? StatusNome { get; set; }
        public int? FilaId { get; set; }
        public string? FilaNome { get; set; }
    }
}
