using System;
using System.Collections.Generic;

namespace ChamaJussaAPI.Domains;

public partial class Localizacao
{
    public int LocalizacaoID { get; set; }

    public string Nome { get; set; } = string.Empty;

    public string Andar { get; set; } = string.Empty;

    public virtual ICollection<OrdemDeServico> OrdemDeServico { get; set; } = new List<OrdemDeServico>();
}
