namespace Carrito_PNT1.Models
{
    public class Cliente : Usuario
    {
        public int ClienteId { get; set; }
        public string DNI { get; set; }
        
        public List<Compra> Compras { get; set; }
        public List<Carrito> Carritos { get; set; }
    }
}
