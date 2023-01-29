using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Carrito_PNT1.Utils;

namespace Carrito_PNT1.Models

{
    [Table("Usuario")]
    public class Usuario
    {
        [Key]
        public int UsuarioId { get; set; }

        [Required(ErrorMessage = ErrorViewModel.CampoRequerido)]
        public string Nombre { get; set; }

        [Required(ErrorMessage = ErrorViewModel.CampoRequerido)]
        public string Apellido { get; set; }

        [Required(ErrorMessage = ErrorViewModel.CampoRequerido)]
        public string Telefono { get; set; }

        [Required(ErrorMessage = ErrorViewModel.CampoRequerido)]
        public string Direccion { get; set; }

        [Required(ErrorMessage = ErrorViewModel.CampoRequerido)]
        public string Email { get; set; }
        public DateTime FechaAlta { get; set; } = DateTime.Now;
        public string Password { get; set; }

    }
}
