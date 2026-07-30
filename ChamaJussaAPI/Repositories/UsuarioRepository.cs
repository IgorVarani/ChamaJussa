using System;
using System.Collections.Generic;
using System.Linq;
using ChamaJussaAPI.Contexts;
using ChamaJussaAPI.Domains;
using ChamaJussaAPI.Interfaces;

namespace ChamaJussaAPI.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly ChamaJussaContext _context;

        public UsuarioRepository(ChamaJussaContext context)
        {
            _context = context;
        }

        public List<Usuario> Listar()
        {
            return _context.Usuario.ToList();
        }

        public Usuario? ObterPorId(Guid id)
        {
            return _context.Usuario.Find(id);
        }

        public Usuario? ObterPorEmail(string Email)
        {
            return _context.Usuario.FirstOrDefault(u => u.Email == Email);
        }

        public bool EmailExiste(string Email)
        {
            return _context.Usuario.Any(u => u.Email == Email);
        }

        public void Adicionar(Usuario Usuario)
        {
            _context.Usuario.Add(Usuario);
            _context.SaveChanges();
        }
    }
}
