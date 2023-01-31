using Carrito_PNT1.Utils;
using System.ComponentModel.DataAnnotations;

namespace Carrito_PNT1.Models
{
    public class Empleado : Usuario
    {
        [Required(ErrorMessage = ErrorViewModel.CampoRequerido)]
        [MaxLength(50, ErrorMessage = ErrorViewModel.CaracteresMaximos)]
        [MinLength(3, ErrorMessage = ErrorViewModel.CaracteresMinimos)]
        public string Legajo { get; set; }
    }
}
