using Carrito_PNT1.Models;
using Carrito_PNT1.Utils;
using System.ComponentModel.DataAnnotations;

namespace CARRITO_D.ViewModels
{
    public class NuevaCompra
    {
        public int CompraId { get; set; }


        [Required]
        [DataType(DataType.Currency, ErrorMessage = ErrorViewModel.TipoInvalido)]
        [Range(0, int.MaxValue, ErrorMessage = ErrorViewModel.MinMaxRange)]
        public double Total { get; set; } = 1;

        [DataType(DataType.Date, ErrorMessage = ErrorViewModel.TipoInvalido)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy hh:mm:ss}")]
        [Display(Name = "Fecha de compra")]
        public DateTime Fecha { get; set; } = DateTime.Now;

        [Required(ErrorMessage = ErrorViewModel.CampoRequerido)]
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }

        [Required(ErrorMessage = ErrorViewModel.CampoRequerido)]
        public int CarritoId { get; set; }
        public Carrito Carrito { get; set; }

        public int SucursalId { get; set; }
    }
}
