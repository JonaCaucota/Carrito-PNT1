using Carrito_PNT1.Utils;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace Carrito_PNT1.Models
{
    public class CarritoItem
    {
        [Key]
        public int CarritoItemId { get; set; }
        public Carrito Carrito { get; set; }
        [ForeignKey("Carrito")]
        public int CarritoId { get; set; }
        public Producto Producto { get; set; }
        [ForeignKey("Producto")]
        public int ProductoId { get; set; }
        [Required(ErrorMessage = ErrorViewModel.CampoRequerido)]
        [Range(1, int.MaxValue, ErrorMessage = ErrorViewModel.MinMaxRange)]
        public int Cantidad { get; set; } = 0;

        [DataType(DataType.Currency, ErrorMessage = ErrorViewModel.TipoInvalido)]
        public double ValorUnitario
        {
            get
            {
                double resultado = 0;
                if (Producto != null)
                {
                    resultado = Producto.PrecioVigente;
                }
                return resultado;
            }
        }
        [DataType(DataType.Currency, ErrorMessage = ErrorViewModel.TipoInvalido)]
        public double Subtotal { 
            get
            {
                return ValorUnitario * Cantidad;
            }
        }
    }
}