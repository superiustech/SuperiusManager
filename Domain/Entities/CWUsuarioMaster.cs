using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class CWUsuarioMaster
    {
        [Key]
        public string sCdUsuario { get; set; }
        public string sSenha { get; set; }
    }
}
