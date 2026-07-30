using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ChamaJussaAPI.Contexts;
using ChamaJussaAPI.Domains;
using ChamaJussaAPI.Interfaces;

namespace ChamaJussaAPI.Repositories
{
    public class OrdemServicoRepository : IOrdemServicoRepository
    {
        private readonly ChamaJussaContext _context;

        public OrdemServicoRepository(ChamaJussaContext context)
        {
            _context = context;
        }

        public void Adicionar(OrdemDeServico os)
        {
            _context.OrdemDeServico.Add(os);
            _context.SaveChanges();
        }

        public void Atualizar(OrdemDeServico os)
        {
            _context.OrdemDeServico.Update(os);
            _context.SaveChanges();
        }

        public void Deletar(OrdemDeServico os)
        {
            _context.OrdemDeServico.Remove(os);
            _context.SaveChanges();
        }

        public List<OrdemDeServico> ListarPorUsuario(Guid usuarioId)
        {
            return _context.OrdemDeServico
                .Include(os => os.Localizacao)
                .Include(os => os.Usuario)
                .Include(os => os.Status)    // Ajustado para 'Status'
                .Include(os => os.Fila)
                .Where(os => os.UsuarioID == usuarioId)
                .ToList();
        }

        public OrdemDeServico? ObterPorId(int id)
        {
            return _context.OrdemDeServico
                .Include(os => os.Localizacao)
                .Include(os => os.Usuario)
                .Include(os => os.Status)    // Ajustado para 'Status'
                .Include(os => os.Fila)
                .FirstOrDefault(os => os.OSID == id);
        }

        public bool LocalizacaoExiste(int localizacaoId)
        {
            return _context.Localizacao.Any(l => l.LocalizacaoID == localizacaoId);
        }

        public bool StatusExiste(int statusId)
        {
            return _context.StatusOS.Any(s => s.StatusID == statusId);
        }

        public int ObterStatusInicialId()
        {
            var statusAberto = _context.StatusOS
                .FirstOrDefault(s => s.Nome.ToLower() == "aberto" || s.Nome.ToLower() == "aberta");

            if (statusAberto != null)
            {
                return statusAberto.StatusID;
            }

            var primeiroStatus = _context.StatusOS.OrderBy(s => s.StatusID).FirstOrDefault();
            if (primeiroStatus != null)
            {
                return primeiroStatus.StatusID;
            }

            var novoStatus = new StatusOS { Nome = "Aberto" };
            _context.StatusOS.Add(novoStatus);
            _context.SaveChanges();
            return novoStatus.StatusID;
        }

        public int? ObterFilaInicialId()
        {
            var primeiraFila = _context.Fila.OrderBy(f => f.FilaID).FirstOrDefault();
            if (primeiraFila != null)
            {
                return primeiraFila.FilaID;
            }

            var novaFila = new Fila { Nome = "Geral" };
            _context.Fila.Add(novaFila);
            _context.SaveChanges();
            return novaFila.FilaID;
        }
    }
}