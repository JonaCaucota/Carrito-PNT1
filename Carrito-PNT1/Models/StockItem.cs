using Carrito_PNT1.Utils;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace Carrito_PNT1.Models
{
    public class StockItem
    {
        [Key]
        public int StockItemId { get; set; }
        [ForeignKey("Sucursal")]
        [Display(Name = "Sucursal")]
        public Sucursal Sucursal { get; set; }
        public Producto Producto { get; set; }
        [ForeignKey("Producto")]
        [Display(Name = "Producto")]
        public int ProductoId { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = ErrorViewModel.MinMaxRange)]
        public int Cantidad { get; set; }
    }
}
