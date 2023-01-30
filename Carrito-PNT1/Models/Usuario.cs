using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Carrito_PNT1.Utils;
using Microsoft.AspNetCore.Identity;

namespace Carrito_PNT1.Models

{
    public class Usuario : IdentityUser<int>
    {
        [Key]
        public int UsuarioId { get; set; }

        [Required(ErrorMessage = ErrorViewModel.CampoRequerido)]
        [MaxLength(50, ErrorMessage = ErrorViewModel.CaracteresMaximos)]
        [MinLength(3, ErrorMessage = ErrorViewModel.CaracteresMinimos)]
        public string Nombre { get; set; }

        [Required(ErrorMessage = ErrorViewModel.CampoRequerido)]
        [MinLength(3, ErrorMessage = ErrorViewModel.CaracteresMinimos)]
        [MaxLength(25, ErrorMessage = ErrorViewModel.CaracteresMaximos)]
        public string Apellido { get; set; }

        [Required(ErrorMessage = ErrorViewModel.CampoRequerido)]
        [MinLength(3, ErrorMessage = ErrorViewModel.CaracteresMinimos)]
        [MaxLength(25, ErrorMessage = ErrorViewModel.CaracteresMaximos)]
        public string Telefono { get; set; }
        [Required(ErrorMessage = ErrorViewModel.CampoRequerido)]
        [MinLength(3, ErrorMessage = ErrorViewModel.CaracteresMinimos)]
        [MaxLength(25, ErrorMessage = ErrorViewModel.CaracteresMaximos)]
        public string DNI { get; set; }

        [Required(ErrorMessage = ErrorViewModel.CampoRequerido)]
        [MinLength(3, ErrorMessage = ErrorViewModel.CaracteresMinimos)]
        [MaxLength(25, ErrorMessage = ErrorViewModel.CaracteresMaximos)]
        public string Direccion { get; set; }

        [Required(ErrorMessage = ErrorViewModel.CampoRequerido)]
        [MinLength(3, ErrorMessage = ErrorViewModel.CaracteresMinimos)]
        [MaxLength(25, ErrorMessage = ErrorViewModel.CaracteresMaximos)]
        public override string Email { get; set; }

        [Required(ErrorMessage = ErrorViewModel.CampoRequerido)]
        [MinLength(3, ErrorMessage = ErrorViewModel.CaracteresMinimos)]
        [MaxLength(25, ErrorMessage = ErrorViewModel.CaracteresMaximos)]
        public override string UserName
        {
            get { return base.UserName; }
            set { base.UserName = value; }
        }

        [DataType(DataType.Date, ErrorMessage = ErrorViewModel.CampoRequerido)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        [Display(Name = "Fecha de creacion")]
        public DateTime FechaAlta { get; set; } = DateTime.Now;
   
    }
}
