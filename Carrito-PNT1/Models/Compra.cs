namespace Carrito_PNT1.Models
{
    public class Compra
    {
        public int CompraId { get; set; }
        public Cliente Cliente { get; set; }
        public int ClienteId { get; set; }
        public Carrito Carrito { get; set; }
        public int CarritoId { get; set; }  
        public double Total { get; set; }

    }
}