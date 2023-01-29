using System.Numerics;

namespace Carrito_PNT1.Models
{
    public class StockItem
    {
        public Sucursal Sucursal { get; set; }
        public Producto Producto { get; set; }
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }
    }
}
