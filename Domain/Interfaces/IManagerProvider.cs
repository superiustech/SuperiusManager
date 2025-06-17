namespace Domain.Interfaces
{
    public interface IManagerProvider
    {
        string? ConsultarUsuario();
        void SetarUsuario(string tenantId);
    }
}