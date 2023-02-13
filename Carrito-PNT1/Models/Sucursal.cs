using Carrito_PNT1.Utils;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Carrito_PNT1.Models
{
    public class Sucursal
    {
        [Display(Name = "Sucursal")]
        public int SucursalId { get; set; }
        [StringLength(20, MinimumLength = 4, ErrorMessage = ErrorViewModel.MinMaxRange)]
        public string Nombre { get; set; }
        [StringLength(20, MinimumLength = 4, ErrorMessage = ErrorViewModel.MinMaxRange)]
        public string Direccion { get; set; }
        [DataType(DataType.PhoneNumber, ErrorMessage = ErrorViewModel.TipoInvalido)]
        public int Telefono { get; set; }
        [DataType(DataType.EmailAddress, ErrorMessage = ErrorViewModel.TipoInvalido)]
        public string Email { get; set; }
        public List<StockItem>  StockItems { get; set; }
    }
}