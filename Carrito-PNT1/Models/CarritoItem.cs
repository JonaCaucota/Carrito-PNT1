using System.Numerics;

namespace Carrito_PNT1.Models
{
    public class CarritoItem
    {
        public int CarritoItemId { get; set; }
        public Carrito Carrito { get; set; }
        public int CarritoId { get; set; } 
        public Producto Producto { get; set; }
        public int ProductoId { get; set; }
        public double ValorUnitario { get; set; }
        public int Cantidad  { get; set; }
        public double Subtotal { get; set; }
    }
}