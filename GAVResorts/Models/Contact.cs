using System.ComponentModel.DataAnnotations;

namespace GAVResorts.Models
{
    public class Contact
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo 'Nome' é obrigatório.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O campo 'Telefone' é obrigatório.")]
        public string Telefone { get; set; }

        [Required(ErrorMessage = "O campo 'Email' é obrigatório.")]
        public string Email { get; set; }
    }
}
