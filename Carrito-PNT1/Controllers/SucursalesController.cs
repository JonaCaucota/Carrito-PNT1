using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Carrito_PNT1.Models;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace Carrito_PNT1.Controllers
{
    public class SucursalsController : Controller
    {
        private readonly DbContext _context;

        public SucursalsController(DbContext context)
        {
            _context = context;
        }

        // GET: Sucursals
        public async Task<IActionResult> Index()
        {
            return View(await _context.Sucursal.ToListAsync());
        }

        // GET: Sucursals/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Sucursal == null)
            {
                return NotFound();
            }

            var sucursal_con_items = _context.StockItem
                .Include(c => c.Sucursal)
                .Include(c => c.Producto)
                .Where(c => c.SucursalId == id);

            if (sucursal_con_items.Any())
            {
                return View(nameof(DetailsConStock), sucursal_con_items);
            }


            var sucursal = _context.Sucursal.FirstOrDefault(c => c.SucursalId == id);
            if (sucursal == null)
            {
                return NotFound();
            }

            return View(sucursal);
        }

        // GET: Sucursals/Create
        [Authorize(Roles = "Empleado")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Sucursals/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Empleado")]
        public async Task<IActionResult> Create([Bind("SucursalId,Nombre,Direccion,Telefono,Email")] Sucursal sucursal)
        {
            if (ModelState.IsValid)
            {
                if (_context.Sucursal.FirstOrDefault(c => c.Nombre == sucursal.Nombre) == null)
                {
                    _context.Add(sucursal);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("Nombre", "Ya hay una sucursal con ese Nombre,\nIngrese otro");
                }
            }
            return View(sucursal);
        }

        // GET: Sucursals/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Sucursal == null)
            {
                return NotFound();
            }

            var sucursal = await _context.Sucursal.FindAsync(id);
            if (sucursal == null)
            {
                return NotFound();
            }
            return View(sucursal);
        }

        // POST: Sucursals/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SucursalId,Nombre,Direccion,Telefono,Email")] Sucursal sucursal)
        {
            if (id != sucursal.SucursalId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sucursal);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SucursalExists(sucursal.SucursalId))
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
            return View(sucursal);
        }

        // GET: Sucursals/Delete/5
        [Authorize(Roles = "Empleado")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Sucursal == null)
            {
                return NotFound();
            }

            var sucursal = await _context.Sucursal
                .FirstOrDefaultAsync(m => m.SucursalId == id);
            if (sucursal == null)
            {
                return NotFound();
            }

            return View(sucursal);
        }

        // POST: Sucursals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Sucursal == null)
            {
                return Problem("Entity set 'DbContext.Sucursal'  is null.");
            }
            var sucursal = await _context.Sucursal.FindAsync(id);
            if (sucursal != null)
            {
                _context.Sucursal.Remove(sucursal);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Empleado")]
        public async Task<IActionResult> SumarProducto(int? id)
        {
            if (id == null || _context.Sucursal == null)
            {
                return NotFound();
            }

            var sucursal = await _context.Sucursal.FindAsync(id);
            if (sucursal == null)
            {
                return NotFound();
            }

            return RedirectToAction("Create", "StocksItems", new { id = id });
        }

        private bool SucursalExists(int id)
        {
            return _context.Sucursal.Any(e => e.SucursalId == id);
        }

        public IActionResult DetailsConStock(IEnumerable<StockItem> stock)
        {
            return View(stock);
        }
    }
}
