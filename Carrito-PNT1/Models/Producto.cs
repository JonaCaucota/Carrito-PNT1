namespace Carrito_PNT1.Models
{
    public class Producto
    {
        public string nombre { get; set; }
        public string descripcion { get;set; }
        public double precioVigente { get; set; }
        public Boolean activo { get; set; }
        public Categoria categoria { get; set; }
    }
}