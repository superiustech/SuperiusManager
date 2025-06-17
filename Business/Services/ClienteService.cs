using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain.Entities.ViewModel;
using Domain.Entities.Uteis;
using Domain.Interfaces;
using Domain.Entities;
using Domain.ViewModel;
using Domain.Entities.Enum;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
using System.Text.RegularExpressions;
using Azure.Core;

namespace Business.Services
{
    public class ClienteService : ICliente
    {
        private readonly IClienteRepository _clienteRepository;
        private readonly IEntidadeLeituraRepository _entidadeLeituraRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpClient _httpClient;
        public ClienteService(IClienteRepository clienteRepository, IEntidadeLeituraRepository entidadeLeituraRepository, IHttpClientFactory factory, IHttpContextAccessor httpContextAccessor)
        {
            _clienteRepository = clienteRepository;
            _entidadeLeituraRepository = entidadeLeituraRepository;
            _httpClient = factory.CreateClient("CoreApi");
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<List<DTOCliente>> PesquisarClientes()
        {
            try
            {
                List<DTOCliente> lstDTOClientes = new List<DTOCliente>();
                List<CWCliente> lstClientes = await _entidadeLeituraRepository.PesquisarTodos<CWCliente>();

                lstDTOClientes.AddRange(lstClientes.Select(x => new DTOCliente()
                {
                   CodigoCliente = x.nCdCliente,
                   NomeCliente = x.sNmCliente ?? string.Empty,
                   CpfCnpj = x.CNPJ ?? string.Empty,
                   Status = x.bFlAtivo
                }));

                return lstDTOClientes;
            }
            catch
            {
                throw;
            }
        }
        public async Task<DTORetorno> CadastrarCliente(DTOCliente oDTOCliente)
        {
            try
            {
                var clienteExistente = await _entidadeLeituraRepository.Consultar<CWCliente>(x => x.sNmCliente == oDTOCliente.NomeCliente);

                if (clienteExistente is not null)
                {
                    throw new ExceptionCustom($"Cliente {oDTOCliente.NomeCliente} já existente no sistema.");
                }

                var cliente = await _clienteRepository.CadastrarCliente(new CWCliente
                {
                    sNmCliente = oDTOCliente.NomeCliente,
                    CNPJ = oDTOCliente.CpfCnpj,
                    bFlAtivo = oDTOCliente.Status
                });

                oDTOCliente.Usuario.TenantId = cliente.nCdCliente.ToString();

                var tokenHeader = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString();
                var match = Regex.Match(tokenHeader ?? "", @"Bearer\s+(.*)", RegexOptions.IgnoreCase);

                if (!match.Success) throw new ExceptionCustom("Token de autenticação não encontrado ou mal formatado.");

                var token = match.Groups[1].Value.Trim();

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                _httpClient.DefaultRequestHeaders.Add("MASTER_TENANTID", oDTOCliente.CodigoCliente.ToString());
                _httpClient.DefaultRequestHeaders.Add("MASTER_TENANTBASE", oDTOCliente.NomeCliente.ToUpper());

                var responseInfra = await _httpClient.PostAsJsonAsync("api/Infra/MigrarTodos", new List<DTOCliente>
                {
                    new()
                    {
                        CodigoCliente = oDTOCliente.CodigoCliente,
                        NomeCliente = oDTOCliente.NomeCliente,
                        CpfCnpj = oDTOCliente.CpfCnpj,
                        Email = oDTOCliente.Email,
                        Status = oDTOCliente.Status 
                    }
                });

                var responseUsuario = await _httpClient.PostAsJsonAsync("api/Usuario/Usuario", oDTOCliente.Usuario);
                var responsePerfil = await _httpClient.PostAsJsonAsync("api/Usuario/AssociarDesassociarPerfis", new { Codigo = oDTOCliente.Usuario.Usuario, CodigosAssociacao = "1"});

                await VerificarResposta(responseInfra, "Migração de Infraestrutura");
                await VerificarResposta(responseUsuario, "Criação de Usuário");
                await VerificarResposta(responsePerfil, "Associação de Perfil");

                return new DTORetorno
                {
                    Status = enumSituacao.Sucesso,
                    Mensagem = "Operação efetuada com sucesso!",
                    Id = cliente.nCdCliente
                };
            }
            catch (ExceptionCustom ex)
            {
                return new DTORetorno { Mensagem = ex.Message, Status = enumSituacao.Erro };
            }
            catch (Exception ex)
            {
                return new DTORetorno { Mensagem = $"Erro interno: {ex.Message}", Status = enumSituacao.Erro };
            }
        }
        private async Task VerificarResposta(HttpResponseMessage response, string contexto)
        {
            if (!response.IsSuccessStatusCode)
            {
                var erro = await response.Content.ReadAsStringAsync();
                throw new ExceptionCustom($"Erro em {contexto}: {erro}");
            }

            var retorno = await response.Content.ReadFromJsonAsync<DTORetorno>();
            if (retorno?.Status == enumSituacao.Erro)
            {
                throw new ExceptionCustom($"Erro de negócio em {contexto}: {retorno.Mensagem}");
            }
        }
        public async Task<DTORetorno> EditarCliente(DTOCliente oDTOCliente)
        {
            try
            {
                CWCliente cliente = await _entidadeLeituraRepository.Consultar<CWCliente>(x => x.sNmCliente == oDTOCliente.NomeCliente || x.nCdCliente == oDTOCliente.CodigoCliente) ?? throw new ExceptionCustom($"Cliente {oDTOCliente.NomeCliente} não existente no sistema.");

                cliente = await _clienteRepository.CadastrarCliente(new CWCliente()
                {
                    nCdCliente = cliente.nCdCliente,
                    sNmCliente = oDTOCliente.NomeCliente,
                    CNPJ = oDTOCliente.CpfCnpj,
                    bFlAtivo = oDTOCliente.Status
                });

                return new DTORetorno() { Status = enumSituacao.Sucesso, Mensagem = "Operação efetuada com sucesso!", Id = cliente.nCdCliente };
            }
            catch (ExceptionCustom ex)
            {
                return new DTORetorno() { Mensagem = ex.Message, Status = enumSituacao.Erro };
            }
            catch (Exception ex)
            {
                #if DEBUG
                return new DTORetorno() { Mensagem = ex.Message, Status = enumSituacao.Erro };
                #endif
                return new DTORetorno() { Mensagem = "Houve um erro não previsto ao processar sua solicitação", Status = enumSituacao.Erro };
            }
        }
    }
}