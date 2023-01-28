namespace Carrito_PNT1.Models
{
    public class Cliente : Usuario
    {
        
        public string dni { get; set; }
        
        public List<Compra> compras { get; set; }
        public Carrito carrito { get; set; }
    }
}
