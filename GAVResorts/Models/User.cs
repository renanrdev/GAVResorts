using System.ComponentModel.DataAnnotations;

namespace GAVResorts.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo 'Usuario' é obrigatório.")]
        public string Usuario { get; set; }

        [Required(ErrorMessage = "O campo 'Senha' é obrigatório.")]
        public string Senha { get; set; }

        [Required(ErrorMessage = "O campo 'TipoUsuario' é obrigatório.")]
        [RegularExpression("(ADM|USR)", ErrorMessage = "O 'TipoUsuario' deve ser 'ADM' ou 'USR'.")]
        public string TipoUsuario { get; set; } // ADM ou USR
    }
}
