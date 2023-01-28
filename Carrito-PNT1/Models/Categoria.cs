namespace Carrito_PNT1.Models
{
    public class Categoria
    {
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public List<Producto> productos { get; set; }
    }
}