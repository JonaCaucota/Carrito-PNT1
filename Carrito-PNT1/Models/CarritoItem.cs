using System.Numerics;

namespace Carrito_PNT1.Models
{
    public class CarritoItem
    {
        public Carrito carrito { get; set; }
        public Producto producto { get; set; }
        public double valorUnitario { get; set; }
        public BigInteger cantidad  { get; set; }
        public double subtotal { get; set; }
    }
}