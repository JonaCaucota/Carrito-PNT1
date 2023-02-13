using Carrito_PNT1.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CARRITO_D.Controllers
{
    public class PreCarga : Controller
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly DbContext _context;
        private readonly List<string> roles = new List<string>() {"Cliente", "Empleado"};

        public PreCarga(UserManager<Usuario> userManager, DbContext context, SignInManager<Usuario> signInManager)
        {
            this._userManager = userManager;
            this._context = context;
        }


        public IActionResult Seed()
        {
            CrearEmpleados().Wait();
            CrearClientes().Wait();
            CrearCategoria().Wait();
            CrearProductos();
            CrearSucursales();
            CrearStockItems();

            return RedirectToAction("Index", "Home", new { mensaje="Proceso de Seed Finalizado"});
        }

        
        

        private async Task CrearCategoria()
        {
            Categoria categoria1 = new Categoria()
            {
                Nombre = "Camisetas",
                Descripcion = "Es una prenda de ropa interior de abrigo por lo general de mangas cortas, cuello redondo o en forma de 'V'",
                Productos = new List<Producto>()
            };
            Categoria categoria2 = new Categoria()
            {
                Nombre = "Pantalones",
                Descripcion = "Prenda de vestir que se ajusta a la cintura y llega generalmente hasta el pie , cubriendo cada pierna separadamente",
                Productos = new List<Producto>()
            };
            Categoria categoria3 = new Categoria()
            {
                Nombre = "Shorts",
                Descripcion = "pantalones cortos de toda la vida que se reinventan año tras año para adaptarse a las nuevas tendencias que dicta la industria de la moda",
                Productos = new List<Producto>()
            };
            Categoria categoria4 = new Categoria()
            {
                Nombre = "Buzos",
                Descripcion = "Prenda deportiva que cubre el torso, generalmente con capucha",
                Productos = new List<Producto>()
            };
            _context.Categoria.Add(categoria1);
            _context.Categoria.Add(categoria2);
            _context.Categoria.Add(categoria3);
            _context.Categoria.Add(categoria4);
            _context.SaveChanges();

        }

        private Categoria encontrarCategoria(string nombreCategoria)
        {
            return _context.Categoria.First(c => c.Nombre == nombreCategoria);
        }
        private void CrearProductos()
        {
            if (_context.Categoria.Any())
            {
                Producto producto1 = new Producto()
                {
                    Nombre = "Camiseta",
                    CategoriaId = encontrarCategoria("Camisetas").CategoriaId,
                    Activo = true,
                    PrecioVigente = 8000,
                    Descripcion = "Camiseta Beige con logo de Klouth en el centro, minimalista",
                    Foto = "FotoCamiseta.png"
                };
                
                Producto producto2 = new Producto()
                {
                    Nombre = "Pantalon",
                    CategoriaId = encontrarCategoria("Pantalones").CategoriaId,
                    Activo = false,
                    PrecioVigente = 5000,
                    Descripcion = "Pantalon de jean marca Klouth",
                    Foto = "FotoPantalon.png"
                };
                Producto producto3 = new Producto()
                {
                    Nombre = "Short",
                    CategoriaId = encontrarCategoria("Shorts").CategoriaId,
                    Activo = true,
                    PrecioVigente = 2500,
                    Descripcion = "Short negro marca Klouth, con hebilla color dorado",
                    Foto = "FotoShort.png"
                };
                Producto producto4 = new Producto()
                {
                    Nombre = "Buzo",
                    CategoriaId = encontrarCategoria("Buzos").CategoriaId,
                    Activo = true,
                    PrecioVigente = 10000,
                    Descripcion = "Buzo negro con logo de Klouth, con capucha y detalles dorados",
                    Foto = "FotoBuzo.png"
                };
               
                _context.Producto.Add(producto1);
                _context.Producto.Add(producto2);
                _context.Producto.Add(producto3);
                _context.Producto.Add(producto4);
                _context.SaveChanges();
            }   
        }

        

        private void CrearSucursales()
        {
            Sucursal sucursal = new Sucursal()
            {
                Direccion = "Av. Libertador 1900",
                Nombre = "Klouth Olivos",
                Telefono = 1147903409,
                Email = "klouth.olivos@klouth.com.ar",
                StockItems = new List<StockItem>()
            };

            Sucursal sucursal2 = new Sucursal()
            {
                Direccion = "Av. Elflein 894",
                Nombre = "Klouth Martinez",
                Telefono = 1147904859,
                Email = "klouth.martinez@klouth.com.ar",
                StockItems = new List<StockItem>()
            };

            _context.Sucursal.Add(sucursal);
            _context.Sucursal.Add(sucursal2);
            _context.SaveChanges();
        }

        private void CrearStockItems()
        {
            StockItem stockitem1 = new StockItem()
            {
                Cantidad = 2,
                ProductoId = _context.Producto.First(c => c.Nombre == "Camiseta").ProductoId,
                SucursalId = _context.Sucursal.First().SucursalId
            };

            StockItem stockitem2 = new StockItem()
            {
                Cantidad = 2,
                ProductoId = _context.Producto.First(c => c.Nombre == "Pantalon").ProductoId,
                SucursalId = _context.Sucursal.First(c => c.Nombre == "Klouth Martinez").SucursalId
            };

            agregarASucursal(stockitem1);
            agregarASucursal(stockitem2);

            _context.StockItem.Add(stockitem1);
            _context.StockItem.Add(stockitem2);
            _context.SaveChanges();
        }

        private void agregarASucursal(StockItem stockitem)
        {
            _context.Sucursal.Include(c => c.StockItems).First(c => c.SucursalId == stockitem.SucursalId).StockItems.Add(stockitem);
        }

        private async Task CrearClientes()
        {

            Cliente clienteNuevo = new
                Cliente()
            {
                UserName = "Cliente1",
                Email = "cliente1@ort.edu.ar",
                Nombre = "Pedro",
                Apellido = "Picapiedra",
                DNI = 45233213,
                Direccion = "Vicente Lopez 789",
                Telefono = 1123456789,
                FechaAlta = DateTime.Now,
                Foto = "Homero-foto-perfil.jpg"

                
               
            };

            //clienteNuevo.Carritos.Add(_context.Carritos.First());

            var resultadoCreate = await _userManager.CreateAsync(clienteNuevo, "Password");

            if (resultadoCreate.Succeeded)
            {
                Carrito carritoNuevo = new Carrito(clienteNuevo.Id);

                await _userManager.AddToRoleAsync(clienteNuevo, "CLIENTE");

                _context.Carrito.Add(carritoNuevo);
                _context.SaveChanges();
            }
        }

        private async Task CrearEmpleados()
        {
            Empleado empleadoNuevo = new
                Empleado()
            {
                UserName = "Empleado1",
                Email = "empleado1@ort.edu.ar",
                Nombre = "Pablo",
                Apellido = "Picapiedra",
                Direccion = "CABA, Belgrano 2033",
                Telefono = 1109876543,
                Legajo = 109234,
                FechaAlta = DateTime.Now,
                Foto = "Moe-foto-perfil.jpg"
            };

            var resultadoCreate = await _userManager.CreateAsync(empleadoNuevo, "Password");

            if (resultadoCreate.Succeeded)
            {
                await _userManager.AddToRoleAsync(empleadoNuevo, "EMPLEADO");
            }
        }

    }
}
