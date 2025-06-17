
using Domain.Entities;
using Domain.Entities.ViewModel;
namespace Domain.Interfaces
{
    public interface IClienteRepository
    {
        public Task<CWCliente> CadastrarCliente(CWCliente oCWCliente);
    }
}