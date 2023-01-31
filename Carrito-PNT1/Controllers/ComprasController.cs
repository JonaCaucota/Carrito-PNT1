using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Carrito_PNT1.Models;

namespace Carrito_PNT1.Controllers
{
    public class ComprasController : Controller
    {
        private readonly DbContext _context;

        public ComprasController(DbContext context)
        {
            _context = context;
        }

        // GET: Compras
        public async Task<IActionResult> Index()
        {
            var dbContext = _context.Compra.Include(c => c.Carrito).Include(c => c.Cliente);
            return View(await dbContext.ToListAsync());
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
        public async Task<IActionResult> Create([Bind("CompraId,ClienteId,CarritoId,Total,Fecha")] Compra compra)
        {
            if (ModelState.IsValid)
            {
                _context.Add(compra);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CarritoId"] = new SelectList(_context.Carrito, "CarritoId", "CarritoId", compra.CarritoId);
            ViewData["ClienteId"] = new SelectList(_context.Cliente, "UsuarioId", "Apellido", compra.ClienteId);
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
