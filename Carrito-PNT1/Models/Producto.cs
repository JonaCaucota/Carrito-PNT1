using Carrito_PNT1.Utils;
using System.ComponentModel.DataAnnotations;

namespace Carrito_PNT1.Models
{
    public class Producto
    {
        [Key]
        public int ProductoId { get; set; }
        [MaxLength(25, ErrorMessage = ErrorViewModel.CaracteresMaximos)]
        public string Nombre { get; set; }
        [MaxLength(200, ErrorMessage = ErrorViewModel.CaracteresMaximos)]
        public string Descripcion { get;set; }
        [DataType(DataType.Currency, ErrorMessage = ErrorViewModel.TipoInvalido)]
        [Range(1, int.MaxValue, ErrorMessage = ErrorViewModel.MinMaxRange)]
        [Display(Name = "Valor por unidad")]
        public double PrecioVigente { get; set; }
        public Boolean Activo { get; set; }
        public Categoria Categoria { get; set; }
        [Required(ErrorMessage = ErrorViewModel.CampoRequerido)]
        public int CategoriaId { get; set; }

        public List<CarritoItem> CarritosItem { get; set; }

        public List<StockItem> StocksItem { get; set; }

        public string Foto { get; set; } = "product-default.png";
    }
}