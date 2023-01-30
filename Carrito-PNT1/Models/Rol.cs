using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Carrito_PNT1.Models
{
    public class Rol : IdentityRole<int>
    {
  
        public Rol() : base() { }
        public Rol(string name) : base(name) { }

        
        [Display(Name = "Nombre")]
        public override string Name
        {
            get { return base.Name; }
            set { base.Name = value; }
        }

        public override string NormalizedName
        {
            get => base.NormalizedName;
            set => base.NormalizedName = value;
        }

    }
}
