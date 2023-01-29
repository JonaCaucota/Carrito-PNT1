namespace Carrito_PNT1.Models
{
    public class Sucursal
    {
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public List<StockItem>  StockItems { get; set; }
    }
}