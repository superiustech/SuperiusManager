using Domain.Entities.ViewModel;
using Domain.ViewModel;
namespace Domain.Interfaces
{
    public interface ICliente
    {
        Task<List<DTOCliente>> PesquisarClientes();
        Task<DTORetorno> CadastrarCliente(DTOCliente oDTOCliente);
        Task<DTORetorno> EditarCliente(DTOCliente oDTOCliente);
    }
}