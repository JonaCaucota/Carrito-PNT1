namespace Carrito_PNT1.Models
{
    public class Sucursal
    {
        public string nombre { get; set; }
        public string direccion { get; set; }
        public string telefono { get; set; }
        public string email { get; set; }
        public List<StockItem>  stockItems { get; set; }
    }
}