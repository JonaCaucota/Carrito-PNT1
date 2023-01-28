namespace Carrito_PNT1.Models
{
    public class Compra
    {
        public Cliente cliente { get; set; }
        public Carrito carrito { get; set; }
        public double total { get; set; }

    }
}