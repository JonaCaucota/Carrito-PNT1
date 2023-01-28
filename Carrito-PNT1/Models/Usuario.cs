using System.ComponentModel.DataAnnotations;

namespace Carrito_PNT1.Models
{
    public class Usuario
    {
        public string nombre { get; set; }
        public string apellido { get; set; }
        public string telefono { get; set; }
        public string direccion { get; set; }
        public string email { get; set; }
        public DateTime fechaAlta { get; set; }
        public string password { get; set; }

    }
}
