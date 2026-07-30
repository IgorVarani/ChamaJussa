using System;

namespace ChamaJussaAPI.DTOs.UsuarioDto
{
    public class LerUsuarioDto
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public int NIF { get; set; }
        public string Email { get; set; } = string.Empty;
    }
}
