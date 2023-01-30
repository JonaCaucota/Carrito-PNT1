using Carrito_PNT1.Utils;
using System.ComponentModel.DataAnnotations;

namespace Carrito_PNT1.Models
{
    public class Compra
    {
        public int CompraId { get; set; }
        public Cliente Cliente { get; set; }
        [Required(ErrorMessage = ErrorViewModel.CampoRequerido)]
        public int ClienteId { get; set; }
        public Carrito Carrito { get; set; }
        [Required(ErrorMessage = ErrorViewModel.CampoRequerido)]
        public int CarritoId { get; set; }
        [Required]
        [DataType(DataType.Currency, ErrorMessage = ErrorViewModel.TipoInvalido)]
        [Range(0, int.MaxValue, ErrorMessage = ErrorViewModel.MinMaxRange)]
        public double Total { get; set; }

        [DataType(DataType.Date, ErrorMessage = ErrorViewModel.TipoInvalido)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy hh:mm:ss}")]
        [Display(Name = "Fecha de Compra")]
        public DateTime Fecha { get; set; } = DateTime.Now;

    }
}