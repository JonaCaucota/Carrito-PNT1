using Carrito_PNT1.Utils;
using System.ComponentModel.DataAnnotations;

namespace Carrito_PNT1.Models
{
    public class Carrito
    {
        public int CarritoId { get; set; }
        public Boolean Activo { get; set; }
        public Cliente Cliente { get; set; }
        [Required(ErrorMessage = ErrorViewModel.CampoRequerido)]
        public int ClienteId { get; set; }
        public List<CarritoItem> CarritoItems { get; set; }

        [DataType(DataType.Currency, ErrorMessage = ErrorViewModel.TipoInvalido)]
        public double Subtotal { get {
                double resultado = 0;
                if (CarritoItems != null)
                {

                    foreach (var item in CarritoItems)
                    {
                        resultado += item.Subtotal;
                    }
                }
                return resultado;
            } }

        public Carrito(int clienteId)
        {
            this.Activo = true;
            this.CarritoItems = new List<CarritoItem>();
            this.ClienteId = clienteId;
        }

    }
}