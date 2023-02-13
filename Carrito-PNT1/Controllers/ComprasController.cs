using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Carrito_PNT1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using CARRITO_D.ViewModels;

namespace Carrito_PNT1.Controllers
{
    [Authorize]
    public class ComprasController : Controller
    {
        private readonly DbContext _context;
        private readonly UserManager<Usuario> _userManager;

        public ComprasController(DbContext context, UserManager<Usuario> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Compras
        public async Task<IActionResult> Index(int? id)
        {
            if (id == null)
            {
                var carritoContext = _context.Compra.Include(c => c.Cliente).Include(c => c.Carrito).OrderBy(c => c.Fecha);
                return View(await carritoContext.ToListAsync());
            }
            else
            {
                var carritoContext = _context.Compra.Include(c => c.Cliente).Include(c => c.Carrito).Where(c => c.ClienteId == id).OrderBy(c => c.Fecha);
                return View(carritoContext);
            }
        }

        // GET: Compras/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Compra == null)
            {
                return NotFound();
            }

            var compra = await _context.Compra
                .Include(c => c.Carrito)
                .Include(c => c.Cliente)
                .FirstOrDefaultAsync(m => m.CompraId == id);
            if (compra == null)
            {
                return NotFound();
            }

            return View(compra);
        }

        // GET: Compras/Create
        public IActionResult Create()
        {
            ViewData["CarritoId"] = new SelectList(_context.Carrito, "CarritoId", "CarritoId");
            ViewData["ClienteId"] = new SelectList(_context.Cliente, "UsuarioId", "Apellido");
            return View();
        }

        // POST: Compras/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SucursalId,CompraId,Total,CarritoId,ClienteId")] NuevaCompra compra)
        {
            Sucursal sucursal = _context.Sucursal.Include(c => c.StockItems).First(c => c.SucursalId == compra.SucursalId);
            List<CarritoItem> items = _context.Carrito.Include(c => c.CarritoItems).First(c => c.ClienteId == compra.ClienteId && c.Activo).CarritoItems;
            if (!hayStock(sucursal, items))
            {
                List<Sucursal> sucursalesConStock = _context.Sucursal.Include(c => c.StockItems).ToList().Where(c => hayStock(c, items)).ToList();

                if (!sucursalesConStock.Any())
                {
                    return RedirectToAction("Details", "Carritos", new { id = compra.ClienteId, msg = "No hay sucursales con stock de los productos seleccionados" });
                }

                ModelState.AddModelError(string.Empty, "Dicha sucursal no tiene stock de todos los productos pedidos");
                ViewData["TotalValue"] = 0;
                ViewData["ClienteId"] = compra.ClienteId;
                ViewData["CarritoId"] = _context.Carrito.First(c => c.ClienteId == compra.ClienteId && c.Activo).CarritoId;
                ViewData["Sucursales"] = new SelectList(sucursalesConStock, "SucursalId", "Nombre");
                return View(compra);
            }

            if (ModelState.IsValid)
            {
                Sucursal sucursalNueva = _context.Sucursal.Find(compra.SucursalId);
                //var carrito = _context.Carritos.Include(c => c.CarritoItems).First(c => c.CarritoId == compra.CarritoId);
                if (sucursalNueva == null)
                {
                    ModelState.AddModelError(string.Empty, "Sucursal no encontrada");
                    return View(compra);
                }
                var carritoI = _context.CarritoItem.Include(c => c.Producto).Where(c => c.CarritoId == compra.CarritoId);
                if (carritoI != null)
                {
                    double total = 0;
                    foreach (var item in carritoI)
                    {
                        total += item.Subtotal;
                    }

                    Compra compraNueva = new Compra()
                    {
                        Total = total,
                        CarritoId = compra.CarritoId,
                        ClienteId = compra.ClienteId
                    };

                    desactivarCarrito(compra.CarritoId);
                    crearNuevoCarrito(compra.ClienteId);
                    eliminarItemsDeStock(sucursalNueva, items);

                    _context.Compras.Add(compraNueva);
                    await _context.SaveChangesAsync();
                    compra.Total = total;
                    compra.CompraId = compraNueva.CompraId;
                    compra.Fecha = DateTime.Now;
                    return RedirectToAction(nameof(Agradecimiento), compra);
                }

            }
            ViewData["TotalValue"] = 0;
            ViewData["ClienteId"] = compra.ClienteId;
            ViewData["CarritoId"] = _context.Carritos.First(c => c.ClienteId == compra.ClienteId && c.Activo).CarritoId;
            ViewData["Sucursales"] = new SelectList(_context.Sucursales, "SucursalId", "Nombre");
            return View(compra);
        }

        // GET: Compras/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Compra == null)
            {
                return NotFound();
            }

            var compra = await _context.Compra.FindAsync(id);
            if (compra == null)
            {
                return NotFound();
            }
            ViewData["CarritoId"] = new SelectList(_context.Carrito, "CarritoId", "CarritoId", compra.CarritoId);
            ViewData["ClienteId"] = new SelectList(_context.Cliente, "UsuarioId", "Apellido", compra.ClienteId);
            return View(compra);
        }

        // POST: Compras/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CompraId,ClienteId,CarritoId,Total,Fecha")] Compra compra)
        {
            if (id != compra.CompraId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(compra);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompraExists(compra.CompraId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CarritoId"] = new SelectList(_context.Carrito, "CarritoId", "CarritoId", compra.CarritoId);
            ViewData["ClienteId"] = new SelectList(_context.Cliente, "UsuarioId", "Apellido", compra.ClienteId);
            return View(compra);
        }

        // GET: Compras/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Compra == null)
            {
                return NotFound();
            }

            var compra = await _context.Compra
                .Include(c => c.Carrito)
                .Include(c => c.Cliente)
                .FirstOrDefaultAsync(m => m.CompraId == id);
            if (compra == null)
            {
                return NotFound();
            }

            return View(compra);
        }

        // POST: Compras/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Compra == null)
            {
                return Problem("Entity set 'DbContext.Compra'  is null.");
            }
            var compra = await _context.Compra.FindAsync(id);
            if (compra != null)
            {
                _context.Compra.Remove(compra);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CompraExists(int id)
        {
            return _context.Compra.Any(e => e.CompraId == id);
        }
    }
}
