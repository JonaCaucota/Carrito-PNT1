using System.Numerics;

namespace Carrito_PNT1.Models
{
    public class StockItem
    {
        public Sucursal sucursal { get; set; }
        public Producto producto { get; set; }
        public BigInteger cantidad { get; set; }
    }
}
