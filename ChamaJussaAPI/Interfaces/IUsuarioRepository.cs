using System;
using System.Collections.Generic;
using ChamaJussaAPI.Domains;

namespace ChamaJussaAPI.Interfaces
{
    public interface IUsuarioRepository
    {
        List<Usuario> Listar();
        Usuario? ObterPorId(Guid id);
        Usuario? ObterPorEmail(string Email);
        bool EmailExiste(string Email);
        void Adicionar(Usuario Usuario);
    }
}
