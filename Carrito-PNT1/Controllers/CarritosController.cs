using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Carrito_PNT1.Models;
using Microsoft.AspNetCore.Identity;

namespace Carrito_PNT1.Controllers
{
    public class CarritosController : Controller
    {
        private readonly DbContext _context;
        private readonly UserManager<Usuario> _userManager;

        public CarritosController(DbContext context, UserManager<Usuario> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

       

        // GET: Carritos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Carrito == null)
            {
                return NotFound();
            }

            var carrito = await _context.Carrito
                .Include(c => c.Cliente)
                .FirstOrDefaultAsync(m => m.CarritoId == id);
            if (carrito == null)
            {
                return NotFound();
            }

            return View(carrito);
        }

        public async Task<IActionResult> LimpiarCarrito(int? id)
        {
            if (id == null || _context.Carrito == null)
            {
                return NotFound();
            }


            var carrito = await _context.Carrito
                .Include(c => c.Cliente)
                .Include(c => c.CarritoItems)
                .FirstOrDefaultAsync(m => m.CarritoId == id);

            if (carrito == null)
            {
                return NotFound();
            }

            for (int i = carrito.CarritoItems.Count(); i > 0; i--)
            {
                var item = carrito.CarritoItems[0];

                carrito.CarritoItems.Remove(item);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Carritos", new { id = carrito.ClienteId });
        }

        public async Task<IActionResult> DetallesProducto(int? id)
        {
            if (id == null || _context.Producto == null)
            {
                return NotFound();
            }

            var producto = await _context.Producto
                .Include(p => p.Categoria)
                .FirstOrDefaultAsync(m => m.ProductoId == id);
            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }
    }
}
