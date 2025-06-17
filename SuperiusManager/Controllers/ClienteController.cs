using Domain.Entities.Enum;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Domain.Entities.ViewModel;
using Domain.Entities.Uteis;
using Microsoft.AspNetCore.Authorization;
using Domain.ViewModel;
namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClienteController : ControllerBase
    {
        private readonly ILogger<ClienteController> _logger;
        private readonly ICliente _cliente;

        public ClienteController( ILogger<ClienteController> logger, ICliente cliente)
        {
            _logger = logger;
            _cliente = cliente;
        }

        [HttpGet("Clientes")]
        public async Task<IActionResult> Clientes()
        {
            try
            {
                return Ok(await _cliente.PesquisarClientes());
            }
            catch (ExceptionCustom ex)
            {
                return BadRequest(new DTORetorno { Status = enumSituacao.Erro, Mensagem = ex.Message });
            }
            catch (Exception ex)
            {
                #if DEBUG
                return BadRequest(new DTORetorno { Status = enumSituacao.Erro, Mensagem = ex.Message });
                #endif
                return BadRequest(new DTORetorno { Status = enumSituacao.Erro, Mensagem = "Houve um erro não previsto ao processar sua solicitação" });
            }
        }
        [HttpPost("Cliente")]
        public async Task<IActionResult> Cliente(DTOCliente oDTOCliente)
        {
            try
            {
                return Ok(await _cliente.CadastrarCliente(oDTOCliente));
            }
            catch (ExceptionCustom ex)
            {
                return BadRequest(new DTORetorno { Status = enumSituacao.Erro, Mensagem = ex.Message });
            }
            catch (Exception ex)
            {
                #if DEBUG
                return BadRequest(new DTORetorno { Status = enumSituacao.Erro, Mensagem = ex.Message });
                #endif
                return BadRequest(new DTORetorno { Status = enumSituacao.Erro, Mensagem = "Houve um erro não previsto ao processar sua solicitação" });
            }
        }
        [HttpPut("Cliente")]
        public async Task<IActionResult> EditarCliente(DTOCliente oDTOCliente)
        {
            try
            {
                return Ok(await _cliente.EditarCliente(oDTOCliente));
            }
            catch (ExceptionCustom ex)
            {
                return BadRequest(new DTORetorno { Status = enumSituacao.Erro, Mensagem = ex.Message });
            }
            catch (Exception ex)
            {
                #if DEBUG
                return BadRequest(new DTORetorno { Status = enumSituacao.Erro, Mensagem = ex.Message });
                #endif
                return BadRequest(new DTORetorno { Status = enumSituacao.Erro, Mensagem = "Houve um erro não previsto ao processar sua solicitação" });
            }
        }
    }
}