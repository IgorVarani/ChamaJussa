using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ChamaJussaAPI.Applications.Services;
using ChamaJussaAPI.DTOs.OrdemServicoDto;
using ChamaJussaAPI.Exceptions;

namespace ChamaJussaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdemServicoController : ControllerBase
    {
        private readonly OrdemServicoService _service;

        public OrdemServicoController(OrdemServicoService service)
        {
            _service = service;
        }

        private Guid ObterUsuarioIdLogado()
        {
            string? idTexto = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(idTexto))
            {
                throw new DomainException("Usuário não autenticado");
            }

            return Guid.Parse(idTexto);
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<LerOrdemServicoDto>> Adicionar([FromForm] CriarOrdemServicoDto osDto)
        {
            try
            {
                Guid UsuarioId = ObterUsuarioIdLogado();
                var osCriada = await _service.AdicionarAsync(osDto, UsuarioId);
                return StatusCode(201, osCriada);
            }
            catch (DomainException ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        [HttpGet]
        public ActionResult<List<LerOrdemServicoDto>> Listar()
        {
            try
            {
                Guid UsuarioId = ObterUsuarioIdLogado();
                var ordens = _service.ListarPorUsuario(UsuarioId);
                return Ok(ordens);
            }
            catch (DomainException ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public ActionResult<LerOrdemServicoDto> ObterPorId(int id)
        {
            try
            {
                var os = _service.ObterPorId(id);
                return Ok(os);
            }
            catch (DomainException ex)
            {
                return NotFound(new { mensagem = ex.Message });
            }
        }

        [HttpGet("{id}/imagem")]
        [AllowAnonymous]
        public ActionResult ObterImagem(int id)
        {
            try
            {
                string? urlOuImagem = _service.ObterImagem(id);
                if (string.IsNullOrWhiteSpace(urlOuImagem))
                {
                    return NotFound(new { mensagem = "Esta OS não possui imagem." });
                }

                if (urlOuImagem.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
                    urlOuImagem.StartsWith("https://", StringComparison.OrdinalIgnoreCase) ||
                    urlOuImagem.StartsWith("/"))
                {
                    return Redirect(urlOuImagem);
                }

                return Ok(new { url = urlOuImagem });
            }
            catch (DomainException ex)
            {
                return NotFound(new { mensagem = ex.Message });
            }
        }

        [HttpGet("Usuario/{UsuarioId}")]
        public ActionResult<List<LerOrdemServicoDto>> ListarPorUsuario(Guid UsuarioId)
        {
            try
            {
                var ordens = _service.ListarPorUsuario(UsuarioId);
                return Ok(ordens);
            }
            catch (DomainException ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        [HttpPatch("{id}/status")]
        [AllowAnonymous]
        public ActionResult<LerOrdemServicoDto> AtualizarStatus(int id, [FromBody] AtualizarStatusDto dto)
        {
            try
            {
                var osAtualizada = _service.AtualizarStatus(id, dto.StatusId);
                return Ok(osAtualizada);
            }
            catch (DomainException ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<LerOrdemServicoDto>> Editar(int id, [FromForm] EditarOrdemServicoDto dto)
        {
            try
            {
                var osAtualizada = await _service.EditarAsync(id, dto);
                return Ok(osAtualizada);
            }
            catch (DomainException ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public ActionResult Deletar(int id)
        {
            try
            {
                _service.Deletar(id);
                return NoContent();
            }
            catch (DomainException ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }
    }
}
