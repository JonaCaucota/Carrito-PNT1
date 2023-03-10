using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Carrito_PNT1.Models;
using Microsoft.AspNetCore.Authorization;

namespace Carrito_PNT1.Controllers
{
    public class ProductosController : Controller
    {
        private readonly DbContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public ProductosController(DbContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: Productos
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            ViewData["Categorias"] = _context.Categoria.ToList().DistinctBy(c => c.Nombre);
            var carritoContext = _context.Producto.Include(p => p.Categoria);
            return View(await carritoContext.ToListAsync());
        }

        // GET: Productos/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
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

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "EMPLEADO")]
        public async Task<IActionResult> Create([Bind("Id,CategoriaId,Activo,Nombre,Descripcion,PrecioVigente")] Producto producto)
        {
            if (ModelState.IsValid)
            {
                _context.Add(producto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoriaId"] = new SelectList(_context.Categoria, "CategoriaId", "Nombre", producto.CategoriaId);
            return View(producto);
        }

        // GET: Productos/Create
        [Authorize(Roles = "EMPLEADO")]
        public IActionResult Create()
        {
            ViewData["CategoriaId"] = new SelectList(_context.Categoria, "CategoriaId", "Nombre");
            return View();
        }

        // GET: Productos/Edit/5
        [Authorize(Roles = "EMPLEADO")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Producto == null)
            {
                return NotFound();
            }

            var producto = await _context.Producto.FindAsync(id);
            if (producto == null)
            {
                return NotFound();
            }
            ViewData["CategoriaId"] = new SelectList(_context.Categoria, "CategoriaId", "CategoriaId", producto.CategoriaId);
            return View(producto);
        }

        // POST: Productos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "EMPLEADO")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Activo,Nombre,Descripcion,PrecioVigente,CategoriaId,Foto")] Producto producto)
        {
            if (id != producto.ProductoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(producto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductoExists(producto.ProductoId))
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
            ViewData["Id"] = new SelectList(_context.Categoria, "CategoriaId", "CategoriaId", producto.ProductoId);
            return View(producto);
        }

        // GET: Productos/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

        public async Task<IActionResult> SumarProducto(int? id)
        {
            if (id == null || _context.Producto == null)
            {
                return NotFound();
            }

            var producto = await _context.Producto.FirstAsync(p => p.ProductoId == id);
            if (producto == null)
            {
                return NotFound();
            }

            return RedirectToAction("Create", "CarritosItems", id);
        }

        // POST: Productos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Producto == null)
            {
                return Problem("Entity set 'DbContext.Producto'  is null.");
            }
            var producto = await _context.Producto.FindAsync(id);
            if (producto != null)
            {
                _context.Producto.Remove(producto);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductoExists(int id)
        {
            return _context.Producto.Any(e => e.ProductoId == id);
        }
    }
}
