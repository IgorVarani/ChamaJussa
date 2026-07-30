using System;
using System.Collections.Generic;

namespace ChamaJussaAPI.Domains;

public partial class OrdemDeServico
{
    public int OSID { get; set; }

    public string Nome { get; set; } = string.Empty;

    public DateTime DTCriacao { get; set; }

    public string Descricao { get; set; } = string.Empty;

    public byte[]? Imagem { get; set; }

    public Guid UsuarioID { get; set; }

    public int LocalizacaoID { get; set; }

    public int StatusID { get; set; }

    public int FilaID { get; set; }

    public virtual Fila Fila { get; set; } = null!;

    public virtual Localizacao Localizacao { get; set; } = null!;

    public virtual StatusOS Status { get; set; } = null!;

    public virtual Usuario Usuario { get; set; } = null!;
}
