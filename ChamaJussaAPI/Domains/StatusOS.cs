using System;
using System.Collections.Generic;

namespace ChamaJussaAPI.Domains;

public partial class StatusOS
{
    public int StatusID { get; set; }

    public string Nome { get; set; } = string.Empty;

    public virtual ICollection<OrdemDeServico> OrdemDeServico { get; set; } = new List<OrdemDeServico>();
}
