using Domain.Entities.Uteis;
using System.Text.Json.Serialization;
namespace Domain.Entities.ViewModel
{
    public class DTOUsuario
    {
        [CampoObrigatorio]
        public string Usuario { get; set; }
        [CampoObrigatorio]
        public string NomeUsuario { get; set; }
        public string? Email { get; set; }
        [CampoObrigatorio]
        public string Senha { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string TenantId { get; set; }
    }
}
