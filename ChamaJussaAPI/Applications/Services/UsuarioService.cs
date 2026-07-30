using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using ChamaJussaAPI.Domains;
using ChamaJussaAPI.DTOs.UsuarioDto;
using ChamaJussaAPI.Exceptions;
using ChamaJussaAPI.Interfaces;

namespace ChamaJussaAPI.Applications.Services
{
    public class UsuarioService
    {
        private readonly IUsuarioRepository _repository;

        public UsuarioService(IUsuarioRepository repository)
        {
            _repository = repository;
        }

        private static LerUsuarioDto LerDto(Usuario Usuario)
        {
            return new LerUsuarioDto
            {
                Id = Usuario.Usuario_id,
                Nome = Usuario.nome,
                NIF = Usuario.nif,
                Email = Usuario.Email
            };
        }

        public List<LerUsuarioDto> Listar()
        {
            List<Usuario> Usuarios = _repository.Listar();
            return Usuarios.Select(u => LerDto(u)).ToList();
        }

        public LerUsuarioDto ObterPorId(Guid id)
        {
            Usuario? Usuario = _repository.ObterPorId(id);
            if (Usuario == null)
            {
                throw new DomainException("Usuário não existe.");
            }
            return LerDto(Usuario);
        }

        private static void ValidarEmail(string Email)
        {
            if (string.IsNullOrWhiteSpace(Email) || !Email.Contains("@"))
            {
                throw new DomainException("Email inválido.");
            }
        }

        private static byte[] HashSenha(string Senha)
        {
            if (string.IsNullOrWhiteSpace(Senha))
            {
                throw new DomainException("Senha é obrigatória.");
            }

            using var sha256 = SHA256.Create();
            return sha256.ComputeHash(Encoding.UTF8.GetBytes(Senha));
        }

        public LerUsuarioDto Adicionar(CriarUsuarioDto UsuarioDto)
        {
            ValidarEmail(UsuarioDto.Email);

            if (_repository.EmailExiste(UsuarioDto.Email))
            {
                throw new DomainException("Já existe um usuário com este e-mail.");
            }

            Usuario Usuario = new Usuario
            {
                Usuario_id = Guid.NewGuid(),
                nome = UsuarioDto.Nome,
                nif = UsuarioDto.NIF,
                Email = UsuarioDto.Email,
                Senha = HashSenha(UsuarioDto.Senha)
            };

            _repository.Adicionar(Usuario);

            return LerDto(Usuario);
        }
    }
}
