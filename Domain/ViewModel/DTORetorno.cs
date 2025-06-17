using Domain.Entities.Enum;
namespace Domain.ViewModel
{
    public class DTORetorno
    {
        public enumSituacao? Status { get; set; }
        public string Mensagem { get; set; }
        public object? Id { get; set; }
    }
}
