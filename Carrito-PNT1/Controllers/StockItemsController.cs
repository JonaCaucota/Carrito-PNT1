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
    [Authorize(Roles = "Empleado")]
    public class StockItemsController : Controller
    {
        private readonly DbContext _context;

        public StockItemsController(DbContext context)
        {
            _context = context;
        }

        // GET: StockItems
        public async Task<IActionResult> Index()
        {
            var dbContext = _context.StockItem.Include(s => s.Producto).Include(s => s.Sucursal);
            return View(await dbContext.ToListAsync());
        }

        // GET: StockItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.StockItem == null)
            {
                return NotFound();
            }

            var stockItem = await _context.StockItem
                .Include(s => s.Producto)
                .Include(s => s.Sucursal)
                .FirstOrDefaultAsync(m => m.StockItemId == id);
            if (stockItem == null)
            {
                return NotFound();
            }

            return View(stockItem);
        }

        // GET: StockItems/Create
        public IActionResult Create(int? id)
        {
            ViewData["ProductoId"] = new SelectList(_context.Producto, "Id", "Nombre");
            if (id == null)
            {
                ViewData["SucursalId"] = new SelectList(_context.Sucursal, "SucursalId", "Nombre");
            }
            else
            {
                ViewData["SucursalId"] = _context.Sucursal.Find(id).SucursalId;
            }

            return View();
        }

        // POST: StockItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Cantidad,SucursalId,ProductoId")] StockItem stockItem)
        {
            //ERROR EN SAVE CHANGES PORQUE HAY MIGRACIONES CON LAS QUE NO SE AVANZARON POR ERRORES
            if (ModelState.IsValid)
            {
                if (_context.StockItem.Any(c => c.ProductoId == stockItem.ProductoId))
                {
                    _context.StockItem.First(c => c.ProductoId == stockItem.ProductoId).Cantidad += stockItem.Cantidad;
                }
                else
                {
                    _context.StockItem.Add(stockItem);
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductoId"] = new SelectList(_context.Producto, "Id", "Nombre", stockItem.ProductoId);
            ViewData["SucursalId"] = new SelectList(_context.Sucursal, "SucursalId", "Nombre", stockItem.SucursalId);
            return View(stockItem);
        }

        // GET: StockItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.StockItem == null)
            {
                return NotFound();
            }

            var stockItem = await _context.StockItem.FindAsync(id);
            if (stockItem == null)
            {
                return NotFound();
            }
            ViewData["ProductoId"] = new SelectList(_context.Producto, "ProductoId", "ProductoId", stockItem.ProductoId);
            ViewData["SucursalId"] = new SelectList(_context.Set<Sucursal>(), "SucursalId", "SucursalId", stockItem.SucursalId);
            return View(stockItem);
        }

        // POST: StockItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Cantidad,SucursalId,ProductoId")] StockItem stockItem)
        {
            if (id != stockItem.SucursalId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(stockItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StockItemExists(stockItem.SucursalId))
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
            ViewData["ProductoId"] = new SelectList(_context.Producto, "Id", "Id", stockItem.ProductoId);
            ViewData["SucursalId"] = new SelectList(_context.Sucursal, "SucursalId", "SucursalId", stockItem.SucursalId);
            return View(stockItem);
        }

        // GET: StockItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.StockItem == null)
            {
                return NotFound();
            }

            var stockItem = await _context.StockItem
                .Include(s => s.Producto)
                .Include(s => s.Sucursal)
                .FirstOrDefaultAsync(m => m.StockItemId == id);
            if (stockItem == null)
            {
                return NotFound();
            }

            return View(stockItem);
        }

        // POST: StockItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.StockItem == null)
            {
                return Problem("Entity set 'DbContext.StockItem'  is null.");
            }
            var stockItem = await _context.StockItem.FindAsync(id);
            if (stockItem != null)
            {
                _context.StockItem.Remove(stockItem);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StockItemExists(int id)
        {
            return _context.StockItem.Any(e => e.StockItemId == id);
        }
    }
}
