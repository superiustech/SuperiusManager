using Domain.Entities.Uteis;
using System.Text.Json.Serialization;
namespace Domain.Entities.ViewModel
{
    public class DTOCliente
    {
        public long CodigoCliente { get; set; }
        [CampoObrigatorio]
        public string NomeCliente { get; set; }
        public string CpfCnpj { get; set; }
        public string Email { get; set; }
        public bool Status { get; set; }
        public DTOUsuario Usuario { get; set; }
    }
}
