using Domain.Interfaces;
namespace Business.Services
{ 
    public class ManagerProvider : IManagerProvider
    {
        private string? _codigoUsuario;
        public string? ConsultarUsuario() => _codigoUsuario;
        public void SetarUsuario(string codigoUsuario) => _codigoUsuario = codigoUsuario;
    }
}