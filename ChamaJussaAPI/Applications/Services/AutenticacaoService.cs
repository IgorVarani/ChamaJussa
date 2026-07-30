using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using ChamaJussaAPI.Applications.Autenticacao;
using ChamaJussaAPI.Domains;
using ChamaJussaAPI.DTOs.AutenticacaoDto;
using ChamaJussaAPI.Exceptions;
using ChamaJussaAPI.Interfaces;

namespace ChamaJussaAPI.Applications.Services
{
    public class AutenticacaoService
    {
        private readonly IUsuarioRepository _repository;
        private readonly GeradorTokenJwt _tokenJwt;

        public AutenticacaoService(IUsuarioRepository repository, GeradorTokenJwt tokenJwt)
        {
            _repository = repository;
            _tokenJwt = tokenJwt;
        }

        private static byte[] HashSenha(string Senha)
        {
            using var sha256 = SHA256.Create();
            return sha256.ComputeHash(Encoding.UTF8.GetBytes(Senha));
        }

        private static bool VerificarSenha(string SenhaDigitada, byte[] SenhaHashBanco)
        {
            return HashSenha(SenhaDigitada).SequenceEqual(SenhaHashBanco);
        }

        public TokenDto Login(LoginDto loginDto)
        {
            Usuario? Usuario = _repository.ObterPorEmail(loginDto.Email);

            if (Usuario == null)
            {
                throw new DomainException("E-mail ou Senha inválidos");
            }

            if (!VerificarSenha(loginDto.Senha, Usuario.Senha))
            {
                throw new DomainException("E-mail ou Senha inválidos");
            }

            var token = _tokenJwt.GerarToken(Usuario);

            return new TokenDto { Token = token };
        }
    }
}
