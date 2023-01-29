using System.ComponentModel.DataAnnotations;

namespace Carrito_PNT1.Models
{
    public class Carrito
    {
        public int CarritoId { get; set; }
        public Boolean Activo { get; set; }
        public Cliente Cliente { get; set; }
        public int ClienteId { get; set; }
        public List<CarritoItem> CarritoItems { get; set; }
        public double Subtotal { get; set; }

    }
}