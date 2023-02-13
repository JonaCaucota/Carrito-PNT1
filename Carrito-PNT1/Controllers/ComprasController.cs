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

                    _context.Compra.Add(compraNueva);
                    await _context.SaveChangesAsync();
                    compra.Total = total;
                    compra.CompraId = compraNueva.CompraId;
                    compra.Fecha = DateTime.Now;
                    return RedirectToAction(nameof(Agradecimiento), compra);
                }

            }
            ViewData["TotalValue"] = 0;
            ViewData["ClienteId"] = compra.ClienteId;
            ViewData["CarritoId"] = _context.Carrito.First(c => c.ClienteId == compra.ClienteId && c.Activo).CarritoId;
            ViewData["Sucursales"] = new SelectList(_context.Sucursal, "SucursalId", "Nombre");
            return View(compra);
        }

        private void eliminarItemsDeStock(Sucursal sucursalNueva, List<CarritoItem> items)
        {
            foreach (var item in items)
            {
                StockItem itemDeStock = _context.StockItem.First(c => c.SucursalId == sucursalNueva.SucursalId && c.ProductoId == item.ProductoId);
                sucursalNueva.StockItems.Remove(itemDeStock);
            }

        }

        private bool hayStock(Sucursal sucursal, List<CarritoItem> items)
        {
            bool hayStock = true;
            int i = 0;

            while (i < items.Count && hayStock)
            {
                foreach (CarritoItem item in items)
                {
                    if (!sucursal.StockItems.Any(c => c.ProductoId == item.ProductoId))
                    {
                        hayStock = false;
                        break;
                    }
                    i++;
                }
            }


            return hayStock;
        }

        private void crearNuevoCarrito(int id)
        {
            Carrito carritoNuevo = new Carrito(id);
            _context.Carrito.Add(carritoNuevo);
        }

        private void desactivarCarrito(int carritoId)
        {
            _context.Carrito.Find(carritoId).Activo = false;
        }

        public IActionResult Agradecimiento(NuevaCompra compra)
        {
            ViewData["Mensaje"] = "Muchas gracias por confiar en nosotros!";
            ViewData["Sucursal"] = _context.Sucursal.Find(compra.SucursalId);

            return View(compra);
        }
    }
}
