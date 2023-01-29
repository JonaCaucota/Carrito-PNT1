namespace Carrito_PNT1.Models
{
    public class Producto
    {
        public int ProductoId { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get;set; }
        public double PrecioVigente { get; set; }
        public Boolean Activo { get; set; }
        public Categoria Categoria { get; set; }
        public int CategoriaId { get; set; }
    }
}