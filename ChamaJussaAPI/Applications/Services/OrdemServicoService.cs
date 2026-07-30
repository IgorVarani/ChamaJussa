using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChamaJussaAPI.Domains;
using ChamaJussaAPI.DTOs.OrdemServicoDto;
using ChamaJussaAPI.Exceptions;
using ChamaJussaAPI.Interfaces;

namespace ChamaJussaAPI.Applications.Services
{
    public class OrdemServicoService
    {
        private readonly IOrdemServicoRepository _repository;
        private readonly IStorageService _storageService;

        public OrdemServicoService(IOrdemServicoRepository repository, IStorageService storageService)
        {
            _repository = repository;
            _storageService = storageService;
        }

        private static LerOrdemServicoDto ConverterParaDto(OrdemDeServico os)
        {
            return new LerOrdemServicoDto
            {
                OsId = os.OSID,
                NomeItem = os.Nome,
                Solicitante = os.UsuarioID,
                SolicitanteNome = os.Usuario?.Nome,
                DtCriacao = os.DTCriacao,
                LocalizacaoId = os.LocalizacaoID,
                LocalizacaoNome = os.Localizacao != null ? $"{os.Localizacao.Nome} (Andar: {os.Localizacao.Andar})" : null,
                Descricao = os.Descricao,

                // Conversão do byte[] do banco para string Base64 pronta para o Mobile:
                Imagem = os.Imagem != null ? $"data:image/jpeg;base64,{Convert.ToBase64String(os.Imagem)}" : null,

                StatusId = os.StatusID,
                StatusNome = os.Status?.Nome,
                FilaId = os.FilaID,
                FilaNome = os.Fila?.Nome
            };
        }

        public async Task<LerOrdemServicoDto> AdicionarAsync(CriarOrdemServicoDto osDto, Guid usuarioId)
        {
            if (string.IsNullOrWhiteSpace(osDto.NomeItem))
            {
                throw new DomainException("Nome do item é obrigatório.");
            }

            if (string.IsNullOrWhiteSpace(osDto.Descricao))
            {
                throw new DomainException("Descrição é obrigatória.");
            }

            if (osDto.LocalizacaoId.HasValue && !_repository.LocalizacaoExiste(osDto.LocalizacaoId.Value))
            {
                throw new DomainException("A localização informada não existe.");
            }

            // Converte a imagem enviada via IFormFile para byte[] (VARBINARY no banco)
            byte[]? imagemBytes = null;
            if (osDto.Imagem != null && osDto.Imagem.Length > 0)
            {
                imagemBytes = await _storageService.ConverterParaByteArrayAsync(osDto.Imagem);
            }

            int statusIdInicial = _repository.ObterStatusInicialId();
            int? filaIdInicial = _repository.ObterFilaInicialId();

            if (!filaIdInicial.HasValue)
            {
                throw new DomainException("Não foi possível identificar uma fila padrão.");
            }

            OrdemDeServico os = new OrdemDeServico
            {
                Nome = osDto.NomeItem,
                UsuarioID = usuarioId,
                DTCriacao = DateTime.Now,
                LocalizacaoID = osDto.LocalizacaoId ?? 1,
                Descricao = osDto.Descricao,
                Imagem = imagemBytes,
                StatusID = statusIdInicial,
                FilaID = filaIdInicial.Value
            };

            _repository.Adicionar(os);

            // Recarrega a OS do banco de dados para popular as entidades navegacionais
            var osBanco = _repository.ObterPorId(os.OSID);
            return osBanco != null ? ConverterParaDto(osBanco) : ConverterParaDto(os);
        }

        public List<LerOrdemServicoDto> ListarPorUsuario(Guid usuarioId)
        {
            List<OrdemDeServico> ordens = _repository.ListarPorUsuario(usuarioId);
            return ordens.Select(os => ConverterParaDto(os)).ToList();
        }

        public LerOrdemServicoDto ObterPorId(int id)
        {
            OrdemDeServico? os = _repository.ObterPorId(id);
            if (os == null)
            {
                throw new DomainException("Ordem de serviço não encontrada.");
            }
            return ConverterParaDto(os);
        }

        public byte[]? ObterImagem(int id)
        {
            OrdemDeServico? os = _repository.ObterPorId(id);
            if (os == null)
            {
                throw new DomainException("Ordem de serviço não encontrada.");
            }
            return os.Imagem;
        }

        private static bool IsStatusAberto(OrdemDeServico os)
        {
            if (os.Status != null && !string.IsNullOrWhiteSpace(os.Status.Nome))
            {
                return string.Equals(os.Status.Nome, "Aberto", StringComparison.OrdinalIgnoreCase) ||
                       string.Equals(os.Status.Nome, "Aberta", StringComparison.OrdinalIgnoreCase);
            }
            return os.StatusID == 1;
        }

        public async Task<LerOrdemServicoDto> EditarAsync(int id, EditarOrdemServicoDto dto)
        {
            OrdemDeServico? os = _repository.ObterPorId(id);
            if (os == null)
            {
                throw new DomainException("Ordem de serviço não encontrada.");
            }

            if (!IsStatusAberto(os))
            {
                throw new DomainException("A Ordem de Serviço não pode ser editada pois seu status já foi modificado.");
            }

            if (!string.IsNullOrWhiteSpace(dto.NomeItem))
            {
                os.Nome = dto.NomeItem;
            }

            if (!string.IsNullOrWhiteSpace(dto.Descricao))
            {
                os.Descricao = dto.Descricao;
            }

            if (dto.LocalizacaoId.HasValue)
            {
                if (!_repository.LocalizacaoExiste(dto.LocalizacaoId.Value))
                {
                    throw new DomainException("A localização informada não existe.");
                }
                os.LocalizacaoID = dto.LocalizacaoId.Value;
            }

            if (dto.Imagem != null && dto.Imagem.Length > 0)
            {
                os.Imagem = await _storageService.ConverterParaByteArrayAsync(dto.Imagem);
            }

            _repository.Atualizar(os);

            var osAtualizada = _repository.ObterPorId(os.OSID);
            return osAtualizada != null ? ConverterParaDto(osAtualizada) : ConverterParaDto(os);
        }

        public void Deletar(int id)
        {
            OrdemDeServico? os = _repository.ObterPorId(id);
            if (os == null)
            {
                throw new DomainException("Ordem de serviço não encontrada.");
            }

            if (!IsStatusAberto(os))
            {
                throw new DomainException("A Ordem de Serviço não pode ser excluída pois seu status já foi modificado.");
            }

            _repository.Deletar(os);
        }

        public LerOrdemServicoDto AtualizarStatus(int id, int statusId)
        {
            OrdemDeServico? os = _repository.ObterPorId(id);
            if (os == null)
            {
                throw new DomainException("Ordem de serviço não encontrada.");
            }

            if (!_repository.StatusExiste(statusId))
            {
                throw new DomainException("O status informado não existe.");
            }

            os.StatusID = statusId;
            _repository.Atualizar(os);

            var osAtualizada = _repository.ObterPorId(os.OSID);
            return osAtualizada != null ? ConverterParaDto(osAtualizada) : ConverterParaDto(os);
        }
    }
}