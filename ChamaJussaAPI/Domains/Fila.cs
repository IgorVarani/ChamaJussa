using System;
using System.Collections.Generic;

namespace ChamaJussaAPI.Domains;

public partial class Fila
{
    public int FilaID { get; set; }

    public string Nome { get; set; } = string.Empty;

    public virtual ICollection<OrdemDeServico> OrdemDeServico { get; set; } = new List<OrdemDeServico>();
}
