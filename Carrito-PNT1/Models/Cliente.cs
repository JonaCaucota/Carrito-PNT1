namespace Carrito_PNT1.Models
{
    public class Cliente : Usuario
    {  
        public List<Compra>? Compras { get; set; }
        public List<Carrito>? Carritos { get; set; }
    }
}
