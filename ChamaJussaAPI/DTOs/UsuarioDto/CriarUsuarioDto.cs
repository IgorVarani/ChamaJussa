namespace ChamaJussaAPI.DTOs.UsuarioDto
{
    public class CriarUsuarioDto
    {
        public string Nome { get; set; } = string.Empty;
        public int NIF { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
    }
}
