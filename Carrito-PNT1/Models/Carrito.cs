namespace Carrito_PNT1.Models
{
    public class Carrito
    {
        public Boolean activo { get; set; }
        public Cliente cliente { get; set; }
        public List<CarritoItem> carritoItems { get; set; }
        public double subtotal { get; set; }
    }
}